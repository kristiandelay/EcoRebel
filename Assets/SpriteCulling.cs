using UnityEngine;

public class SpriteCulling : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private float spriteHalfWidth;
    private float spriteHalfHeight;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        // Calculate half of the sprite's width and height (in world units)
        spriteHalfWidth = spriteRenderer.bounds.size.x / 2.0f;
        spriteHalfHeight = spriteRenderer.bounds.size.y / 2.0f;
    }

    void Update()
    {
        // Get the camera's bounds in world coordinates
        float camHalfHeight = mainCamera.orthographicSize;
        float camHalfWidth = mainCamera.aspect * camHalfHeight;
        float camLeft = mainCamera.transform.position.x - camHalfWidth;
        float camRight = mainCamera.transform.position.x + camHalfWidth;
        float camTop = mainCamera.transform.position.y + camHalfHeight;
        float camBottom = mainCamera.transform.position.y - camHalfHeight;

        // Check if the sprite is within the camera's bounds
        bool isVisible = (transform.position.x + spriteHalfWidth >= camLeft) &&
                         (transform.position.x - spriteHalfWidth <= camRight) &&
                         (transform.position.y + spriteHalfHeight >= camBottom) &&
                         (transform.position.y - spriteHalfHeight <= camTop);

        // Enable or disable the Sprite Renderer based on visibility
        spriteRenderer.enabled = isVisible;
    }
}
