using DG.Tweening;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class KingMovement : MonoBehaviour
{
	[SerializeField] private float _speed;
	
	private bool _isDead = false;
	private void Update()
	{
		if (!_isDead)
		{
			transform.position += transform.right * (_speed * Time.deltaTime);
		}
	}

	private void OnEnable()
	{
		Messenger.Default.Subscribe <GameOverEvent>(OnGameOver);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe <GameOverEvent>(OnGameOver);
	}

	private void OnGameOver(GameOverEvent obj)
	{
		_isDead = true;
		
		// 1. Stop any current rotation running on this transform
		transform.DOKill();

		// 2. Rotate relative to current local Z axis by 90 degrees
		transform.DOLocalRotate(new Vector3(0, 0, 90), 0.5f, RotateMode.LocalAxisAdd)	
			.SetEase(Ease.OutQuad);
	}
}
