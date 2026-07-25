using System;
using DG.Tweening;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class KingMovement : MonoBehaviour
{
	[SerializeField] private float _speed;
	
	[Tooltip("for walking on stairs")]
	[SerializeField] private float _stepWidth = 0.4f; // Horizontal tread length
	private float _stepHeight => _stepWidth * Mathf.Tan(37.5f * Mathf.Deg2Rad); // Automatically ~0.3f
	
	private bool _isDead = false;

	public enum WalkingState
	{
		Straight, Up, Down
	}
	
	public WalkingState _walkingState = WalkingState.Straight;
	
	private void Update()
	{
		if (_isDead) return;
		switch (_walkingState)
		{
			case WalkingState.Straight:
				WalkStraight();
				break;
			case WalkingState.Up:
				WalkUpStairs();
				break;
			case WalkingState.Down:
				WalkDownStairs();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void WalkStraight()
	{
		transform.position += transform.right * (_speed * Time.deltaTime);
	}
	
	private void WalkUp()
	{
		transform.position += transform.up * (_speed * Time.deltaTime);
	}
	
	private void WalkDown()
	{
		transform.position -= transform.up * (_speed * Time.deltaTime);
	}

	private void WalkUpStairs()
	{
		// Alternate based on travel ratio: 1 unit forward to ~0.75 units up
		float totalCycleTime = (_stepWidth + _stepHeight) / _speed;
		float forwardRatio = _stepWidth / (_stepWidth + _stepHeight);
    
		float currentCycleProgress = (Time.time % totalCycleTime) / totalCycleTime;

		if (currentCycleProgress < forwardRatio)
		{
			WalkStraight();
		}
		else
		{
			WalkUp();
		}
	}

	private void WalkDownStairs()
	{
		// Alternate based on travel ratio: 1 unit forward to ~0.75 units up
		float totalCycleTime = (_stepWidth + _stepHeight) / _speed;
		float forwardRatio = _stepWidth / (_stepWidth + _stepHeight);
    
		float currentCycleProgress = (Time.time % totalCycleTime) / totalCycleTime;

		if (currentCycleProgress < forwardRatio)
		{
			WalkStraight();
		}
		else
		{
			WalkDown();
		}
	}

	private void OnEnable()
	{
		Messenger.Default.Subscribe <GameOverEvent>(OnGameOver);
		Messenger.Default.Subscribe <LoadCheckPointEvent>(OnLoadCheckPoint);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe <GameOverEvent>(OnGameOver);
		Messenger.Default.Unsubscribe <LoadCheckPointEvent>(OnLoadCheckPoint);
	}

	private void OnLoadCheckPoint(LoadCheckPointEvent obj)
	{
		_isDead = false;
		transform.DOKill();
		transform.rotation = Quaternion.identity;
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
