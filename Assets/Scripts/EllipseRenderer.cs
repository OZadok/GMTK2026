using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EllipseRenderer : MonoBehaviour
{
	public int segments = 100;
	public float xAxisRadius = 5f;
	public float yAxisRadius = 3f;

	[ReadOnly] public LineRenderer line;

	[SerializeField] private SpriteRenderer _regularSprite;
	[SerializeField] private SpriteRenderer _pressedSprite;
	[SerializeField] private ParticleSystem _particleSystemBack;
	[SerializeField] private ParticleSystem _particleSystemFront;
	
	[SerializeField] private Color _color;
	
	[SerializeField] private UITimerNumberChange _uiTimerNumberChange;

	[SerializeField] private AudioEvent _pressingAudioEvent;

	private bool _isShowing;

	void Awake()
	{
		line = GetComponent<LineRenderer>();
		CalculateEllipse();
	}

	// Call this if you modify values in real-time
	void OnValidate()
	{
		if (line == null) line = GetComponent<LineRenderer>();
		CalculateEllipse();
	}

	private void Update()
	{
		CalculateEllipse();
	}

	void CalculateEllipse()
	{
		line.positionCount = segments + 1;
		float angleStep = 360f / segments;

		for (int i = 0; i <= segments; i++)
		{
			float progressAngle = i * angleStep * Mathf.Deg2Rad;
            
			// Calculate positions using parametric formula
			float x = transform.position.x + Mathf.Cos(progressAngle) * xAxisRadius;
			float y = transform.position.y + Mathf.Sin(progressAngle) * yAxisRadius;

			line.SetPosition(i, new Vector3(x, y, 0f));
		}
	}

	public void ShowUINUmberTimerChange()
	{
		_uiTimerNumberChange.Show();
	}
	
	public void ToShow(bool toShow, float duration=0.5f)
	{
		_isShowing = toShow;
		var targetAlpha = toShow ? 1f : 0f;
		StartCoroutine(FadeTo(_regularSprite, targetAlpha, duration));
	}
	
	/// <summary>
	/// Fades any passed-in SpriteRenderer to a target alpha.
	/// </summary>
	public IEnumerator FadeTo(SpriteRenderer targetSprite, float targetAlpha, float duration)
	{
		if (targetSprite == null) yield break;

		Color startColor = targetSprite.color;
		float startAlpha = startColor.a;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            
			targetSprite.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            
			yield return null;
		}

		// Guarantee final value
		targetSprite.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
	}

	public void OnPressed(bool isPressed)
	{
		if (!_isShowing) return;
		if (isPressed)
		{
			if (_regularSprite && _regularSprite.gameObject)
			{
				_regularSprite?.gameObject.SetActive(false);
			}

			if (_pressedSprite && _pressedSprite.gameObject)
			{
				_pressedSprite?.gameObject.SetActive(true);
			}
			if (!_particleSystemBack.isPlaying)
			{
				_particleSystemBack.Play();
				_pressingAudioEvent.Play(transform);
			}
			if (!_particleSystemFront.isPlaying)
			{
				_particleSystemFront.Play();
			}

			var color = _color;
			
			Color.RGBToHSV(color, out float h, out float s, out float v);
			s = 100;
			color = Color.HSVToRGB(h, s, v);
			line.startColor = color;
			line.endColor = color;
		}
		else
		{
			if (_regularSprite && _regularSprite.gameObject)
			{
				_regularSprite.gameObject.SetActive(true);
			}
			if (_pressedSprite && _pressedSprite.gameObject)
			{
				_pressedSprite.gameObject.SetActive(false);
			}
			
			if (_particleSystemBack.isPlaying)
			{
				_particleSystemBack.Stop();
			}
			if (_particleSystemFront.isPlaying)
			{
				_particleSystemFront.Stop();
			}
			
			line.startColor = _color;
			line.endColor = _color;
		}
	}
}