using System;
using Events;
using SuperMaxim.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class KingMovement : MonoBehaviour
{
	[SerializeField] private float _speed;
	
	private bool _isDead = false;
	private void Update()
	{
		if (!_isDead)
		{
			transform.position += transform.right * (_speed * Time.deltaTime);
		}
	}

	private void OnEnable()
	{
		Messenger.Default.Subscribe <GameOverEvent>(OnGameOver);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe <GameOverEvent>(OnGameOver);
	}

	private void OnGameOver(GameOverEvent obj)
	{
		_isDead = true;
	}
}
