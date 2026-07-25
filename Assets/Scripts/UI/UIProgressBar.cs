using System;
using System.Collections;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
	[SerializeField] private Slider _slider;
	[SerializeField] private Image _background;
	[SerializeField] private CanvasGroup _canvasGroup;

	private void Start()
	{
		StartCoroutine(FadeCanvasGroup(_canvasGroup, 0, 0));
		//_slider.gameObject.SetActive(false);
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
		StartCoroutine(FadeCanvasGroup(_canvasGroup, 0, 0));
		//_slider.gameObject.SetActive(false);
	}

	private void OnEndWalkInCastle(EndWalkInCastleEvent obj)
	{
		StartCoroutine(FadeCanvasGroup(_canvasGroup, 1, 0.5f));
		//_slider.gameObject.SetActive(true);
	}

	private void OnStartWalkInCastle(StartWalkInCastleEvent obj)
	{
		StartCoroutine(FadeCanvasGroup(_canvasGroup, 0, 0.5f));
		//_slider.gameObject.SetActive(false);
	}

	private void OnLevelProgress(LevelProgressEvent levelProgressEvent)
	{
		_slider.value = levelProgressEvent.LevelProgress;
	}

	public void SwapBackgroundSprite(Sprite sprite)
	{
		_background.sprite = sprite;
	}
	
	/// <summary>
	/// Fades a CanvasGroup's alpha over time.
	/// </summary>
	public IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
	{
		if (canvasGroup == null) yield break;

		float startAlpha = canvasGroup.alpha;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
			yield return null;
		}

		canvasGroup.alpha = targetAlpha;
	}
}
