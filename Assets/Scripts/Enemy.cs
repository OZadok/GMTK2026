using UnityEngine;

public class Enemy : MonoBehaviour
{
	private static readonly int Electrified1 = Animator.StringToHash("Electrified");
	public EnemyType _type;
	
	[SerializeField] private Animator _animator;
	[SerializeField] private GoToKing _goToKing;

	public void Electrified()
	{
		_animator.SetTrigger(Electrified1);
		_goToKing.enabled = false;
	}
}