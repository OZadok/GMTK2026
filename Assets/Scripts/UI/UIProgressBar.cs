using System;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
	[SerializeField] private Slider _slider;
	[SerializeField] private Image _background;

	private void Start()
	{
		_slider.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		Messenger.Default.Subscribe<LevelProgressEvent>(OnLevelProgress);
		Messenger.Default.Subscribe<StartWalkInCastleEvent>(OnStartWalkInCastle);
		Messenger.Default.Subscribe<EndWalkInCastleEvent>(OnEndWalkInCastle);
		Messenger.Default.Subscribe<LoadCheckPointEvent>(OnLoadCheckPoint);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe<LevelProgressEvent>(OnLevelProgress);
		Messenger.Default.Unsubscribe<StartWalkInCastleEvent>(OnStartWalkInCastle);
		Messenger.Default.Unsubscribe<EndWalkInCastleEvent>(OnEndWalkInCastle);
		Messenger.Default.Unsubscribe<LoadCheckPointEvent>(OnLoadCheckPoint);
	}

	private void OnLoadCheckPoint(LoadCheckPointEvent obj)
	{
		_slider.gameObject.SetActive(false);
	}

	private void OnEndWalkInCastle(EndWalkInCastleEvent obj)
	{
		_slider.gameObject.SetActive(true);
	}

	private void OnStartWalkInCastle(StartWalkInCastleEvent obj)
	{
		_slider.gameObject.SetActive(false);
	}

	private void OnLevelProgress(LevelProgressEvent levelProgressEvent)
	{
		_slider.value = levelProgressEvent.LevelProgress;
	}

	public void SwapBackgroundSprite(Sprite sprite)
	{
		_background.sprite = sprite;
	}
}
