using UnityEngine;

public class CameraBoundsWrap : MonoBehaviour
{
    [Header("Camera Reference")]
    [SerializeField] private Camera targetCamera;

    [Header("Wrap Directions")]
    [SerializeField] private bool wrapHorizontal = true;
    [SerializeField] private bool wrapVertical = false;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (targetCamera == null || spriteRenderer == null) return;

        // Get object's bounding box in world space
        Bounds objectBounds = spriteRenderer.bounds;

        // Calculate Camera screen edges in world space
        float camHeight = targetCamera.orthographicSize * 2f;
        float camWidth = camHeight * targetCamera.aspect;

        Vector3 camPos = targetCamera.transform.position;

        float camLeft   = camPos.x - (camWidth / 2f);
        float camRight  = camPos.x + (camWidth / 2f);
        float camBottom = camPos.y - (camHeight / 2f);
        float camTop    = camPos.y + (camHeight / 2f);

        // --- HORIZONTAL WRAPPING ---
        if (wrapHorizontal)
        {
            // Fully past the RIGHT edge -> move to the far LEFT edge
            if (objectBounds.min.x > camRight)
            {
                float objectWidth = objectBounds.size.x;
                transform.position -= new Vector3(camWidth + objectWidth, 0f, 0f);
            }
            // Fully past the LEFT edge -> move to the far RIGHT edge
            else if (objectBounds.max.x < camLeft)
            {
                float objectWidth = objectBounds.size.x;
                transform.position += new Vector3(camWidth + objectWidth, 0f, 0f);
            }
        }

        // --- VERTICAL WRAPPING ---
        if (wrapVertical)
        {
            // Fully past the TOP edge -> move to the far BOTTOM edge
            if (objectBounds.min.y > camTop)
            {
                float objectHeight = objectBounds.size.y;
                transform.position -= new Vector3(0f, camHeight + objectHeight, 0f);
            }
            // Fully past the BOTTOM edge -> move to the far TOP edge
            else if (objectBounds.max.y < camBottom)
            {
                float objectHeight = objectBounds.size.y;
                transform.position += new Vector3(0f, camHeight + objectHeight, 0f);
            }
        }
    }
}