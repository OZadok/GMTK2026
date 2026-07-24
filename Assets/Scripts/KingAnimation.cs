using System.Collections;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class KingAnimation : MonoBehaviour
{
    private static readonly int Hit = Animator.StringToHash("Hit");
    [SerializeField] private Animator _animator;
    
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
        _animator.SetBool(Hit, true);
        yield return null;
		
        /*yield return new WaitUntil(() => 
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
        );*/
        
        _animator.SetBool(Hit, false);
    }
}
