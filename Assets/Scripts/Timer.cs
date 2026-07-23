using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _startTime;

    [ReadOnly] [SerializeField] private float _time;
    private TimerTimeEvent _timerTimeEvent;
    [SerializeField] private float _timeToAddWhenEnemyDestroyed = 5;
    [SerializeField] private float _timeToReduceWhenPressedAndEnemyNotDestroyed = 5;

    private void Start()
    {
        _time = _startTime;
        _timerTimeEvent = new TimerTimeEvent(_time);
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<EnemyDestroyedEvent>(OnEnemyDestroyed);
        Messenger.Default.Subscribe<TryDestroyEnemyAndFailedEvent>(OnTryDestroyEnemyAndFailed);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<EnemyDestroyedEvent>(OnEnemyDestroyed);
        Messenger.Default.Unsubscribe<TryDestroyEnemyAndFailedEvent>(OnTryDestroyEnemyAndFailed);
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time < 0)
        {
            _time = 0;
        }
        _timerTimeEvent.Time = _time;
        Messenger.Default.Publish(_timerTimeEvent);
    }
    
    private void OnEnemyDestroyed(EnemyDestroyedEvent enemyDestroyedEvent)
    {
        if (_time > 0)
        {
            _time += _timeToAddWhenEnemyDestroyed;
        }
    }
    
    private void OnTryDestroyEnemyAndFailed(TryDestroyEnemyAndFailedEvent tryDestroyEnemyAndFailedEvent)
    {
        _time -= _timeToReduceWhenPressedAndEnemyNotDestroyed;
    }
}
