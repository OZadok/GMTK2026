using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
	private static readonly int Electrified1 = Animator.StringToHash("Electrified");
	private static readonly int Attack = Animator.StringToHash("Attack");
	public EnemyType _type;
	
	[SerializeField] private Animator _animator;
	[SerializeField] private GoToKing _goToKing;

	[ReadOnly] public bool _isDead = false;

	private void OnEnable()
	{
		Messenger.Default.Subscribe<EnemyReachesKingEvent>(OnEnemyReachesKing);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe<EnemyReachesKingEvent>(OnEnemyReachesKing);
	}

	private void OnEnemyReachesKing(EnemyReachesKingEvent enemyReachesKingEvent)
	{
		StartCoroutine(AttackAndDie());
	}
	
	private IEnumerator AttackAndDie()
	{
		_animator.SetTrigger(Attack);
		yield return null;
		
		yield return new WaitUntil(() => 
			_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
		);

		Enemies.Instance.RemoveEnemy(gameObject);
	}

	public void Electrified()
	{
		_isDead = true;
		_animator.SetTrigger(Electrified1);
		_goToKing.enabled = false;
	}
}