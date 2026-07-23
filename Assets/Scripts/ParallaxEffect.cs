using UnityEngine;

[DisallowMultipleComponent]
public class ParallaxEffect : MonoBehaviour
{
    [Header("Camera Reference")]
    [Tooltip("If left unassigned, it will default to Camera.main")]
    [SerializeField] private Transform cameraTransform;

    [Header("Parallax Settings")]
    [Tooltip("Multiplier for movement. 0 = static (pinned to screen), 1 = moves normally with camera, >1 = foreground (moves faster than camera)")]
    [SerializeField] private Vector2 parallaxMultiplier = new Vector2(0.5f, 0.5f);

    [Header("Infinite Scrolling (Optional)")]
    [SerializeField] private bool infiniteHorizontal = false;
    [SerializeField] private bool infiniteVertical = false;

    private Vector3 lastCameraPosition;
    private float textureSizeX;
    private float textureSizeY;

    private void Start()
    {
        if (cameraTransform == null)
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogError("ParallaxEffect: No main camera found! Please assign one in the inspector.", this);
                enabled = false;
                return;
            }
        }

        lastCameraPosition = cameraTransform.position;

        // Calculate sprite bounds if infinite scrolling is enabled
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Sprite sprite = spriteRenderer.sprite;
            if (sprite != null)
            {
                textureSizeX = sprite.rect.width / sprite.pixelsPerUnit * transform.localScale.x;
                textureSizeY = sprite.rect.height / sprite.pixelsPerUnit * transform.localScale.y;
            }
        }
    }

    // LateUpdate prevents camera jitter when tracking physics or SmoothDamp targets
    private void LateUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        
        // Apply parallax offset based on camera delta
        transform.position += new Vector3(
            deltaMovement.x * parallaxMultiplier.x,
            deltaMovement.y * parallaxMultiplier.y,
            0f
        );

        lastCameraPosition = cameraTransform.position;

        // Handle infinite horizontal looping
        if (infiniteHorizontal && textureSizeX > 0)
        {
            float cameraDistX = cameraTransform.position.x - transform.position.x;
            if (Mathf.Abs(cameraDistX) >= textureSizeX)
            {
                float offsetPositionX = (cameraDistX % textureSizeX);
                transform.position = new Vector3(cameraTransform.position.x - offsetPositionX, transform.position.y, transform.position.z);
            }
        }

        // Handle infinite vertical looping
        if (infiniteVertical && textureSizeY > 0)
        {
            float cameraDistY = cameraTransform.position.y - transform.position.y;
            if (Mathf.Abs(cameraDistY) >= textureSizeY)
            {
                float offsetPositionY = (cameraDistY % textureSizeY);
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y - offsetPositionY, transform.position.z);
            }
        }
    }
}