using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UITimerNumberChange : MonoBehaviour
{
	[SerializeField] private Image _image;

	private void Start()
	{
		_image.gameObject.SetActive(false);
	}

	public void Show()
	{
		var image = Instantiate(_image, transform);
		image.gameObject.SetActive(true);
		AnimateMoveAndFade(image, image.rectTransform, 0.5f, 0.3f);
	}
	
	
	/// <summary>
    /// Moves a given RectTransform upward while fading its Image component in and out.
    /// </summary>
    /// <param name="targetImage">The UI Image component to fade.</param>
    /// <param name="targetRect">The RectTransform component to move.</param>
    /// <param name="moveDistance">Distance to move up on the Y-axis.</param>
    /// <param name="duration">Total time for the entire move & fade animation.</param>
    /// <param name="peakOpacity">Maximum alpha value during the middle of the fade (0.0 to 1.0).</param>
    public static void AnimateMoveAndFade(Image targetImage, RectTransform targetRect, float moveDistance = 200f, float duration = 2f, float peakOpacity = 1f)
    {
        // 1. Safety check
        if (targetImage == null || targetRect == null)
        {
            Debug.LogWarning("AnimateMoveAndFade: Image or RectTransform parameter is null.");
            return;
        }

        // 2. Kill any active tweens running on these specific components to avoid overlap bugs
        targetRect.DOKill();
        targetImage.DOKill();

        // 3. Store starting position
        Vector2 startPos = targetRect.anchoredPosition;

        // 4. Set initial alpha to 0 (invisible)
        Color initialColor = targetImage.color;
        initialColor.a = 0f;
        targetImage.color = initialColor;

        // 5. Create the DOTween Sequence
        Sequence mySequence = DOTween.Sequence();
        float halfDuration = duration / 2f;

        // --- MOVEMENT ---
        // Move Y relative to starting position
        mySequence.Join(targetRect.DOAnchorPosY(startPos.y + moveDistance, duration).SetEase(Ease.OutQuad));

        // --- FADE IN & FADE OUT ---
        // Step A: Fade In to peak opacity during the first half
        mySequence.Join(targetImage.DOFade(peakOpacity, halfDuration).SetEase(Ease.InQuad));
        
        // Step B: Fade Out to 0 alpha during the second half
        mySequence.Append(targetImage.DOFade(0f, halfDuration).SetEase(Ease.OutQuad));
    }
}
