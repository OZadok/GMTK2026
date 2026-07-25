using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _startTime;

    [ReadOnly] [SerializeField] private float _time;
    private TimerTimeEvent _timerTimeEvent;
    [SerializeField] private float _timeToAddWhenEnemyDestroyed = 5;
    [SerializeField] private float _timeToReduceWhenPressedAndEnemyNotDestroyed = 1;
    [SerializeField] private float _timeToReduceWhenEnemyReachesKing = 2;

    private bool _toRun;

    private void Start()
    {
        _time = _startTime;
        _timerTimeEvent = new TimerTimeEvent(_time);
    }

    public void Init(float startTime, float timeToAddWhenEnemyDestroyed, float timeToReduceWhenPressedAndEnemyNotDestroyed, float timeToReduceWhenEnemyReachesKing)
    {
        _startTime = startTime;
        _time = _startTime;
        _timeToAddWhenEnemyDestroyed = timeToAddWhenEnemyDestroyed;
        _timeToReduceWhenPressedAndEnemyNotDestroyed = timeToReduceWhenPressedAndEnemyNotDestroyed;
        _timeToReduceWhenEnemyReachesKing = timeToReduceWhenEnemyReachesKing;
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<EnemyDestroyedEvent>(OnEnemyDestroyed);
        Messenger.Default.Subscribe<TryDestroyEnemyAndFailedEvent>(OnTryDestroyEnemyAndFailed);
        Messenger.Default.Subscribe<EnemyReachesKingEvent>(OnEnemyReachesKing);
        Messenger.Default.Subscribe<StartWalkInCastleEvent>(OnStartWalkInCastle);
        Messenger.Default.Subscribe<EndWalkInCastleEvent>(OnEndWalkInCastle);
        Messenger.Default.Subscribe<GameOverEvent>(OnGameOver);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<EnemyDestroyedEvent>(OnEnemyDestroyed);
        Messenger.Default.Unsubscribe<TryDestroyEnemyAndFailedEvent>(OnTryDestroyEnemyAndFailed);
        Messenger.Default.Unsubscribe<EnemyReachesKingEvent>(OnEnemyReachesKing);
        Messenger.Default.Unsubscribe<StartWalkInCastleEvent>(OnStartWalkInCastle);
        Messenger.Default.Unsubscribe<EndWalkInCastleEvent>(OnEndWalkInCastle);
        Messenger.Default.Unsubscribe<GameOverEvent>(OnGameOver);
    }

    private void OnGameOver(GameOverEvent obj)
    {
        _toRun = false;
    }

    private void OnEndWalkInCastle(EndWalkInCastleEvent obj)
    {
        _toRun = true;
    }

    private void OnStartWalkInCastle(StartWalkInCastleEvent obj)
    {
        _toRun = false;
    }

    private void OnEnemyReachesKing(EnemyReachesKingEvent obj)
    {
        _time -= _timeToReduceWhenEnemyReachesKing;
        Messenger.Default.Publish(new TimerTimeChangeEvent(-_timeToReduceWhenEnemyReachesKing));
    }

    private void Update()
    {
        if (!_toRun) return;
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
        if (!_toRun) return;
        if (_time > 0)
        {
            _time += _timeToAddWhenEnemyDestroyed;
            Messenger.Default.Publish(new TimerTimeChangeEvent(_timeToAddWhenEnemyDestroyed));
        }
    }
    
    private void OnTryDestroyEnemyAndFailed(TryDestroyEnemyAndFailedEvent tryDestroyEnemyAndFailedEvent)
    {
        if (!_toRun) return;
        _time -= _timeToReduceWhenPressedAndEnemyNotDestroyed;
        Messenger.Default.Publish(new TimerTimeChangeEvent(-_timeToReduceWhenPressedAndEnemyNotDestroyed));
    }
}
