using System;
using Events;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;

public class UiTimer : MonoBehaviour
{
	[Header("References")] 
	[SerializeField] private TMP_Text _timeText;

	private void OnEnable()
	{
		Messenger.Default.Subscribe<TimerTimeEvent>(OnTimerTime);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe<TimerTimeEvent>(OnTimerTime);
	}

	private void OnTimerTime(TimerTimeEvent timerTimeEvent)
	{
		TimeSpan time = TimeSpan.FromSeconds(timerTimeEvent.Time);

		// Format to Minute:Second:Millisecond (MM:ss:ff)
		_timeText.text = time.ToString(@"mm\:ss\.ff");
	}
}
