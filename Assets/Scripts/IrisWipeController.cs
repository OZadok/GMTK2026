using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IrisWipeController : MonoBehaviour
{
	[SerializeField] private Image irisImage;
	[SerializeField] private float duration = 1.2f;

	private Material irisMaterial;
	private static readonly int RadiusID = Shader.PropertyToID("_Radius");
	private static readonly int CenterID = Shader.PropertyToID("_Center");

	private void Awake()
	{
		// Instantiates a unique material copy so other UI isn't affected
		irisMaterial = irisImage.material;
	}

	public void PlayIrisOut(Vector2 normalizedScreenPos, System.Action onComplete = null)
	{
		StartCoroutine(AnimateIris(1.5f, 0.0f, normalizedScreenPos, onComplete));
	}

	public void PlayIrisIn(Vector2 normalizedScreenPos, System.Action onComplete = null)
	{
		StartCoroutine(AnimateIris(0.0f, 1.5f, normalizedScreenPos, onComplete));
	}

	private IEnumerator AnimateIris(float startRadius, float endRadius, Vector2 center, System.Action onComplete)
	{
		irisMaterial.SetVector(CenterID, center);
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			float currentRadius = Mathf.Lerp(startRadius, endRadius, t);
            
			irisMaterial.SetFloat(RadiusID, currentRadius);
			yield return null;
		}

		irisMaterial.SetFloat(RadiusID, endRadius);
		onComplete?.Invoke();
	}
}