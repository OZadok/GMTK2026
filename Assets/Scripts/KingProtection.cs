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

    [Header("Target & Input Settings")]
    [Tooltip("Tag assigned to your Enemy GameObjects.")]
    [SerializeField] private string enemyTag = "Enemy";

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
            var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            
            if (distanceToEnemy >= MinDistance && distanceToEnemy <= MaxDistance)
            {
                DestroyEnemy(enemy);
                break;
            }
        }
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

    // Visualize the valid distance ring in Scene View
    private void OnDrawGizmos()
    {
        Vector3 center = transform.position;

        // Ideal Target Distance (Yellow)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, targetDistance);

        // Max Allowed Outer Bound (Green)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, MaxDistance);

        // Min Allowed Inner Bound (Red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, MinDistance);
    }
}