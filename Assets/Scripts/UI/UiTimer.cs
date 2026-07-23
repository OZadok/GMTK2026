using System;
using Events;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;

public class UiTimer : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField] private TMP_Text _changeTimeText;
	
	[Header("References")] 
	[SerializeField] private TMP_Text _timeText;

	[SerializeField] private Transform _changeTimeParent;

	[Header("Parameters")] [SerializeField]
	private float _timeForChangeTimeToApear = 0.5f;

	private void OnEnable()
	{
		Messenger.Default.Subscribe<TimerTimeEvent>(OnTimerTime);
		Messenger.Default.Subscribe<TimerTimeChangeEvent>(OnTimerTimeChange);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe<TimerTimeEvent>(OnTimerTime);
		Messenger.Default.Unsubscribe<TimerTimeChangeEvent>(OnTimerTimeChange);
	}

	private void OnTimerTimeChange(TimerTimeChangeEvent timerTimeChangeEvent)
	{
		var text = timerTimeChangeEvent.Amount > 0 ? "+" + timerTimeChangeEvent.Amount : $"{timerTimeChangeEvent.Amount}";
		var changeTimeText = Instantiate(_changeTimeText, _changeTimeParent);
		changeTimeText.text = text;
		Destroy(changeTimeText.gameObject, _timeForChangeTimeToApear);
	}

	private void OnTimerTime(TimerTimeEvent timerTimeEvent)
	{
		TimeSpan time = TimeSpan.FromSeconds(timerTimeEvent.Time);

		// Format to Minute:Second:Millisecond (MM:ss:ff)
		_timeText.text = time.ToString(@"mm\:ss\.ff");
	}
}
