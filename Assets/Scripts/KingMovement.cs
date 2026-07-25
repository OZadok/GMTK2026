using System;
using DG.Tweening;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class KingMovement : MonoBehaviour
{
	[SerializeField] public float _speed;
	
	[Tooltip("for walking on stairs")]
	[SerializeField] private float _stepWidth = 0.4f; // Horizontal tread length
	private float _stepHeight => _stepWidth * Mathf.Tan(37.5f * Mathf.Deg2Rad); // Automatically ~0.3f
	
	private bool _isDead = false;

	public enum WalkingState
	{
		Straight, Up, Down
	}
	
	private WalkingState _walkingState = WalkingState.Straight;
	
	private void FixedUpdate()
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
	
	public void StartWalkingStraight()
	{
		_walkingState = WalkingState.Straight;
	}
	

	// Call this when the character enters/starts stair movement
	public void StartWalkingUpStairs()
	{
		_walkingState = WalkingState.Up;
		_stairTimer = 0f;
	}

	private void WalkUpStairs()
	{
		// Advance local timer by frame duration
		_stairTimer += Time.deltaTime;

		float totalCycleTime = (_stepWidth + _stepHeight) / _speed;
		float upRatio = _stepHeight / (_stepWidth + _stepHeight);

		// Progression from 0.0 to 1.0 based on local timer
		float currentCycleProgress = (_stairTimer % totalCycleTime) / totalCycleTime;

		if (currentCycleProgress < upRatio)
		{
			WalkUp();
		}
		else
		{
			WalkStraight();
		}
	}
	
	private float _stairTimer = 0f;
	private bool _isWalkingDownStairs = false;

	// Call this when the character enters/starts stair movement
	public void StartWalkingDownStairs()
	{
		_walkingState = WalkingState.Down;
		_isWalkingDownStairs = true;
		_stairTimer = 0f; // Reset to 0 so you ALWAYS start by going Down
	}

	private void WalkDownStairs()
	{
		if (!_isWalkingDownStairs) return;

		// Advance local timer by frame duration
		_stairTimer += Time.deltaTime;

		float totalCycleTime = (_stepWidth + _stepHeight) / _speed;
		float downRatio = _stepHeight / (_stepWidth + _stepHeight);

		// Progression from 0.0 to 1.0 based on local timer
		float currentCycleProgress = (_stairTimer % totalCycleTime) / totalCycleTime;

		if (currentCycleProgress < downRatio)
		{
			WalkDown();
		}
		else
		{
			WalkStraight();
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
