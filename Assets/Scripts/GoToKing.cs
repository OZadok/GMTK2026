using System;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.Serialization;

public class GoToKing : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("Reference to the King. If unassigned, it searches for 'King' tag.")]
    [SerializeField] private Transform _kingTransform;
    
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private float _stoppingDistance = 1.0f;
    
    [Header("Sprite Settings")]
    [SerializeField] private SpriteRenderer _enemySpriteRenderer;

    private void Start()
    {
        if (_kingTransform == null)
        {
            GameObject kingObj = GameObject.FindGameObjectWithTag("King");
            if (kingObj != null)
            {
                _kingTransform = kingObj.transform;
            }
            else
            {
                Debug.LogWarning($"[GoToKing] No King object assigned or found with tag 'King'!");
            }
        }
    }

    private void Update()
    {
        if (!_kingTransform) return;

        _enemySpriteRenderer.flipX = transform.position.x > _kingTransform.position.x;
        
        // Calculate distance to King
        float distanceToKing = Vector3.Distance(transform.position, _kingTransform.position);

        // Only move if further away than the stopping distance
        if (distanceToKing > _stoppingDistance)
        {
            // Move toward the king
            transform.position = Vector3.MoveTowards(
                transform.position, 
                _kingTransform.position, 
                _moveSpeed * Time.deltaTime
            );
        }
        else
        {
            OnReachKing();
        }
    }

    private void OnReachKing()
    {
        // Add logic here (e.g., Attack, Stop Animation, Game Over)
        Messenger.Default.Publish(new EnemyReachesKingEvent());
        Enemies.Instance.RemoveEnemy(gameObject);
    }
}
