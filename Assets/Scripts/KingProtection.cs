using System;
using System.Collections.Generic;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

public class KingProtection : MonoBehaviour
{
	[Serializable]
	private struct EnemyDestroyer
	{
		public EllipseRenderer _ellipseRenderer;

		[Tooltip("The button pressed to destroy eligible enemies.")]
		public InputAction _destroyKey;

		public EnemyType _enemyType;
	}

	[SerializeField] private List<EnemyDestroyer> _enemyDestroyers;

	[Header("Optional VFX")] [SerializeField]
	private GameObject destroyFxPrefab;

	private void OnEnable()
	{
		foreach (var enemyDestroyer in _enemyDestroyers)
		{
			enemyDestroyer._destroyKey.Enable();
		}
	}

	private void OnDisable()
	{
		foreach (var enemyDestroyer in _enemyDestroyers)
		{
			enemyDestroyer._destroyKey.Disable();
		}
	}

	private void Update()
	{
		foreach (var enemyDestroyer in _enemyDestroyers)
		{
			if (enemyDestroyer._destroyKey.WasPressedThisFrame())
			{
				var isEnemyDestroyed = TryDestroyEnemiesInRange(enemyDestroyer);
				if (!isEnemyDestroyed)
				{
					Messenger.Default.Publish(new TryDestroyEnemyAndFailedEvent());
				}
			}
		}
	}

	private bool TryDestroyEnemiesInRange(EnemyDestroyer enemyDestroyer)
	{
		// 1. Find all active enemies in the scene by tag (no physics required)
		var enemies = Enemies.Instance.GetEnemies(enemyDestroyer._enemyType);

		// Loop backwards so destroying elements won't disrupt array iteration
		foreach (var enemy in enemies)
		{
			if (IsObjectOnEllipse(enemy.transform.position, enemyDestroyer._ellipseRenderer))
			{
				DestroyEnemy(enemy);
				return true;
			}
		}

		return false;
	}

	private bool IsObjectOnEllipse(Vector3 objectPosition, EllipseRenderer ellipseRenderer)
	{
		// 1. Convert object position relative to the ellipse center
		Vector3 localPos = objectPosition - ellipseRenderer.transform.position;

		// Optional: If your ellipse rotates, un-rotate the position relative to the center transform:
		// localPos = Quaternion.Inverse(ellipseCenter.rotation) * localPos;

		// 2. Calculate normalized value
		var xRadius = ellipseRenderer.xAxisRadius;
		var yRadius = ellipseRenderer.yAxisRadius;
		float normalizedVal = (localPos.x * localPos.x) / (xRadius * xRadius)
		                      + (localPos.y * localPos.y) / (yRadius * yRadius);

		// 3. Check if the value is within (1 - threshold) and (1 + threshold)
		return Mathf.Abs(normalizedVal - 1f) <= ellipseRenderer.line.startWidth;
	}

	private void DestroyEnemy(Enemy enemy)
	{
		if (destroyFxPrefab)
		{
			Instantiate(destroyFxPrefab, enemy.transform.position, Quaternion.identity);
		}

		Messenger.Default.Publish(new EnemyDestroyedEvent());
		Debug.Log($"[KingProtection] Destroyed {enemy.name} at distance!");
		Enemies.Instance.RemoveEnemy(enemy);
	}
}