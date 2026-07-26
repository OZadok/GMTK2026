using System;
using System.Collections;
using Events;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiTimer : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField] private TMP_Text _changeTimeText;
	
	[Header("References")] 
	[SerializeField] private TMP_Text _timeText;
	[SerializeField] private Image _flowerImage; 

	[SerializeField] private Transform _changeTimeParent;

	[Header("Parameters")] [SerializeField]
	private float _timeForChangeTimeToApear = 0.5f;

	[SerializeField] private Color _plusColor;
	[SerializeField] private Color _minusColor;

	private void Start()
	{
		_timeText.text = "";
	}

	private void OnEnable()
	{
		Messenger.Default.Subscribe<TimerTimeEvent>(OnTimerTime);
		Messenger.Default.Subscribe<TimerTimeChangeEvent>(OnTimerTimeChange);
		Messenger.Default.Subscribe<StartWalkInCastleEvent>(OnStartWalkInCastle);
		Messenger.Default.Subscribe<LoadCheckPointEvent>(OnLoadCheckPoint);
	}

	private void OnDisable()
	{
		Messenger.Default.Unsubscribe<TimerTimeEvent>(OnTimerTime);
		Messenger.Default.Unsubscribe<TimerTimeChangeEvent>(OnTimerTimeChange);
		Messenger.Default.Unsubscribe<StartWalkInCastleEvent>(OnStartWalkInCastle);
		Messenger.Default.Unsubscribe<LoadCheckPointEvent>(OnLoadCheckPoint);
	}

	private void OnLoadCheckPoint(LoadCheckPointEvent obj)
	{
		_timeText.text = "";
	}

	private void OnStartWalkInCastle(StartWalkInCastleEvent obj)
	{
		_timeText.text = "";
	}

	private Coroutine _colorTextCoroutine;
	private void OnTimerTimeChange(TimerTimeChangeEvent timerTimeChangeEvent)
	{
		if (_colorTextCoroutine != null)
		{
			StopCoroutine(_colorTextCoroutine);
		}
		_colorTextCoroutine = StartCoroutine(ColorText(timerTimeChangeEvent.Amount > 0));
		return;
		var text = timerTimeChangeEvent.Amount > 0 ? "+" + timerTimeChangeEvent.Amount : $"{timerTimeChangeEvent.Amount}";
		var changeTimeText = Instantiate(_changeTimeText, _changeTimeParent);
		changeTimeText.text = text;
		Destroy(changeTimeText.gameObject, _timeForChangeTimeToApear);
	}

	private IEnumerator ColorText(bool isPlus)
	{
		if (isPlus)
		{
			_timeText.color = _plusColor;
		}
		else
		{
			_timeText.color = _minusColor;
		}
		yield return new WaitForSeconds(0.4f);
		TextWhite();
		_colorTextCoroutine = null;
	}

	private void TextWhite()
	{
		_timeText.color = Color.white;
	}

	private void OnTimerTime(TimerTimeEvent timerTimeEvent)
	{
		TimeSpan time = TimeSpan.FromSeconds(timerTimeEvent.Time);

		// Format to Minute:Second:Millisecond (MM:ss:ff)
		_timeText.text = time.ToString(@"mm\:ss\.ff");
	}

	public void SwapFlowerSprite(Sprite flowerSprite)
	{
		_flowerImage.sprite = flowerSprite;
	}
}
