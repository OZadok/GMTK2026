using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class KingProtection : MonoBehaviour
{
    [Header("Distance & Buffer Settings")]
    [Tooltip("Ideal distance from the King where the destroy action works.")]
    [SerializeField] private float targetDistance = 3.0f;

    [Tooltip("Allowed leeway/tolerance above and below targetDistance.")]
    [SerializeField] private float bufferDistance = 1.0f;

    [SerializeField] private EllipseRenderer _ellipseRenderer;

    [Tooltip("The button pressed to destroy eligible enemies.")]
    [SerializeField] private KeyCode destroyKey = KeyCode.E;

    [Header("Optional VFX")]
    [SerializeField] private GameObject destroyFxPrefab;

    // Minimum and Maximum boundaries calculated with buffer
    public float MinDistance => Mathf.Max(0f, targetDistance - bufferDistance);
    public float MaxDistance => targetDistance + bufferDistance;

    private void Update()
    {
        if (Input.GetKeyDown(destroyKey))
        {
            TryDestroyEnemiesInRange();
        }
    }

    private void TryDestroyEnemiesInRange()
    {
        // 1. Find all active enemies in the scene by tag (no physics required)
        var enemies = Enemies.Instance._enemies;

        // Loop backwards so destroying elements won't disrupt array iteration
        foreach (var enemy in enemies)
        {
            if (IsObjectOnEllipse(enemy.transform.position))
            {
                DestroyEnemy(enemy);
                break;
            }
        }
    }

    private bool IsObjectOnEllipse(Vector3 objectPosition)
    {
        // 1. Convert object position relative to the ellipse center
        Vector3 localPos = objectPosition - _ellipseRenderer.transform.position;

        // Optional: If your ellipse rotates, un-rotate the position relative to the center transform:
        // localPos = Quaternion.Inverse(ellipseCenter.rotation) * localPos;

        // 2. Calculate normalized value
        var xRadius = _ellipseRenderer.xAxisRadius; 
        var yRadius = _ellipseRenderer.yAxisRadius; 
        float normalizedVal = (localPos.x * localPos.x) / (xRadius * xRadius) 
                              + (localPos.y * localPos.y) / (yRadius * yRadius);

        // 3. Check if the value is within (1 - threshold) and (1 + threshold)
        return Mathf.Abs(normalizedVal - 1f) <= _ellipseRenderer.line.startWidth;
    }

    private void DestroyEnemy(GameObject enemy)
    {
        if (destroyFxPrefab != null)
        {
            Instantiate(destroyFxPrefab, enemy.transform.position, Quaternion.identity);
        }

        Messenger.Default.Publish(new EnemyDestroyedEvent());
        Debug.Log($"[KingProtection] Destroyed {enemy.name} at distance!");
        Enemies.Instance.RemoveEnemy(enemy);
    }
}