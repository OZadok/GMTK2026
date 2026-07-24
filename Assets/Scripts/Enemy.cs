using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
	private static readonly int Electrified1 = Animator.StringToHash("Electrified");
	public EnemyType _type;
	
	[SerializeField] private Animator _animator;
	[SerializeField] private GoToKing _goToKing;

	[ReadOnly] public bool _isDead = false;
	public void Electrified()
	{
		_isDead = true;
		_animator.SetTrigger(Electrified1);
		_goToKing.enabled = false;
	}
}