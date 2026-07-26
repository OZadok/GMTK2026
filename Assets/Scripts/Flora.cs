using UnityEngine;

public class Flora : MonoBehaviour
{
	private static readonly int Surprised = Animator.StringToHash("Surprised");
	[SerializeField] private Animator _animator;

	public void Surprise()
	{
		_animator.SetTrigger(Surprised);
	}
}
