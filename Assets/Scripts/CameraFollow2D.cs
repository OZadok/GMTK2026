using System;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraFollow2D : MonoBehaviour
{
	[Header("Target & Offset")]
	public Transform target;
	public Vector3 offset = new Vector3(0f, 0f, -10f); // Keep z = -10 so camera doesn't clip

	[Header("Movement Settings")]
	[Range(0.01f, 1f)]
	public float smoothSpeed = 0.125f; // Lower = smoother/slower follow

	[Header("Optional Bounds")]
	public bool enableBounds = false;
	public Vector2 minBounds;
	public Vector2 maxBounds;
	
	[Header("Camera Shake Settings")]
	private float shakeDuration = 0f;
	private float shakeMagnitude = 0.1f;
	private float dampingSpeed = 1.0f;

	private void LateUpdate()
	{
		if (target == null) return;

		// 1. Calculate desired position
		Vector3 targetPos = target.position + offset;

		// 2. Clamp target position within room/level boundaries if enabled
		if (enableBounds)
		{
			targetPos.x = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
			targetPos.y = Mathf.Clamp(targetPos.y, minBounds.y, maxBounds.y);
		}

		// 3. Smoothly interpolate between current position and desired position
		Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        
		// 4. Calculate camera shake displacement offset
		Vector3 shakeOffset = Vector3.zero;
		if (shakeDuration > 0)
		{
			// Generate random 2D offset inside a unit circle
			Vector2 randomPoint = Random.insideUnitCircle * shakeMagnitude;
			shakeOffset = new Vector3(randomPoint.x, randomPoint.y, 0f);

			// Reduce shake duration smoothly over time
			shakeDuration -= Time.deltaTime * dampingSpeed;
		}

		// 5. Apply smooth position + shake offset
		transform.position = smoothedPos + shakeOffset;
	}
	
	/// <summary>
	/// Triggers a camera shake effect.
	/// </summary>
	/// <param name="duration">How long the shake lasts in seconds.</param>
	/// <param name="magnitude">How intense/far the camera vibrates.</param>
	/// <param name="damping">How fast the shake fades out (default = 1.0).</param>
	public void TriggerShake(float duration = 0.2f, float magnitude = 0.2f, float damping = 1.0f)
	{
		shakeDuration = duration;
		shakeMagnitude = magnitude;
		dampingSpeed = damping;
	}

	private void OnEnable()
	{
		Messenger.Default.Subscribe<EnemyReachesKingEvent>(OnEnemyReachesKing);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe<EnemyReachesKingEvent>(OnEnemyReachesKing);
	}

	private void OnEnemyReachesKing(EnemyReachesKingEvent obj)
	{
		TriggerShake(0.3f, 0.15f, 1f);
	}
}