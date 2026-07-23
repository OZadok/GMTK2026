using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	void Start()
	{
		//Time.timeScale = 0;
	}

	private void OnEnable()
	{
		Messenger.Default.Subscribe<StartGameEvent>(OnStartGame);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe<StartGameEvent>(OnStartGame);
	}

	private void OnStartGame(StartGameEvent obj)
	{
		Time.timeScale = 1;
	}
}