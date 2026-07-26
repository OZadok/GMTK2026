using System.Collections;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class KingAnimation : MonoBehaviour
{
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Dead = Animator.StringToHash("Dead");
    [SerializeField] private Animator _animator;
    
    [SerializeField] private UITimerNumberChange _uiTimerNumberChange;
    
    private void OnEnable()
    {
        Messenger.Default.Subscribe<EnemyReachesKingEvent>(OnEnemyReachesKing);
        Messenger.Default.Subscribe<GameOverEvent>(OnGameOver);
        Messenger.Default.Subscribe<LoadCheckPointEvent>(OnLoadCheckPoint);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<EnemyReachesKingEvent>(OnEnemyReachesKing);
        Messenger.Default.Unsubscribe<GameOverEvent>(OnGameOver);
        Messenger.Default.Unsubscribe<LoadCheckPointEvent>(OnLoadCheckPoint);
    }

    private void OnLoadCheckPoint(LoadCheckPointEvent obj)
    {
        _animator.Play($"Walk");
    }

    private void OnGameOver(GameOverEvent obj)
    {
        _animator.SetTrigger(Dead);
    }

    private void OnEnemyReachesKing(EnemyReachesKingEvent enemyReachesKingEvent)
    {
        StartCoroutine(AttackAndDie());
    }
	
    private IEnumerator AttackAndDie()
    {
        _animator.SetBool(Hit, true);
        _uiTimerNumberChange.Show();
        yield return null;
		
        /*yield return new WaitUntil(() => 
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
        );*/
        
        _animator.SetBool(Hit, false);
    }
}
