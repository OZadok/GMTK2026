using System;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class KingSfx : MonoBehaviour
{
	[SerializeField] private AudioEvent _kingDiesAudioEvent;
	[SerializeField] private AudioEvent _kingHitAudioEvent;
	private void OnEnable()
	{
		Messenger.Default.Subscribe<GameOverEvent>(OnGameOver);
		Messenger.Default.Subscribe<EnemyReachesKingEvent>(OnEnemyReachesKing);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe<GameOverEvent>(OnGameOver);
		Messenger.Default.Unsubscribe<EnemyReachesKingEvent>(OnEnemyReachesKing);
	}

	private void OnEnemyReachesKing(EnemyReachesKingEvent obj)
	{
		_kingHitAudioEvent.Play(transform);
	}

	private void OnGameOver(GameOverEvent obj)
	{
		_kingDiesAudioEvent.Play(transform);
	}
}
