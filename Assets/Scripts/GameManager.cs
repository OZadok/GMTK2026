using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	void Start()
	{
		//Time.timeScale = 0;
	}

	private bool isGameOver = false;
	private void OnEnable()
	{
		Messenger.Default.Subscribe<StartGameEvent>(OnStartGame);
		Messenger.Default.Subscribe<TimerTimeEvent>(OnTimerTime);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe<StartGameEvent>(OnStartGame);
		Messenger.Default.Unsubscribe<TimerTimeEvent>(OnTimerTime);
	}

	private void OnTimerTime(TimerTimeEvent timerTimeEvent)
	{
		if (timerTimeEvent.Time <= 0 && !isGameOver)
		{
			isGameOver = true;
			Messenger.Default.Publish(new GameOverEvent());
		}
	}

	private void OnStartGame(StartGameEvent obj)
	{
		Time.timeScale = 1;
	}
}