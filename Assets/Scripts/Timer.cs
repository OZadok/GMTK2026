using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _startTime;

    [ReadOnly] [SerializeField] private float _time;
    private TimerTimeEvent _timerTimeEvent;

    private void Start()
    {
        _time = _startTime;
        _timerTimeEvent = new TimerTimeEvent(_time);
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        _timerTimeEvent.Time = _time;
        Messenger.Default.Publish(_timerTimeEvent);
    }
}
