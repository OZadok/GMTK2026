using UnityEngine;

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
        
		// 4. Apply position
		transform.position = smoothedPos;
	}
}