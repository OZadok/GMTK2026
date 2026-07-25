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
		Messenger.Default.Subscribe<EndWalkInCastleEvent>(OnEndWalkInCastle);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe<StartGameEvent>(OnStartGame);
		Messenger.Default.Unsubscribe<TimerTimeEvent>(OnTimerTime);
		Messenger.Default.Unsubscribe<EndWalkInCastleEvent>(OnEndWalkInCastle);
	}

	private void OnEndWalkInCastle(EndWalkInCastleEvent obj)
	{
		isGameOver = false;
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