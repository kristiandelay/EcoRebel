using UnityEngine;

public class RaycastColorPicker : MonoBehaviour
{
    void Update()
    {
        // Cast a ray below the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);

        if (hit.collider != null)
        {
            // Check if the collider is a trigger on the background sprite
            if (hit.collider.gameObject.CompareTag("Background"))
            {
                // Get the contact point of the collider
                Vector2 contactPoint = hit.point;

                // Get the BoxCollider2D component of the collider
                BoxCollider2D boxCollider = hit.collider.GetComponent<BoxCollider2D>();

                if (boxCollider != null)
                {
                    // Compute the texture coordinate of the contact point
                    Vector2 center = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y);
                    Vector2 textureCoord = center - contactPoint;
                    textureCoord.x /= boxCollider.size.x;
                    textureCoord.y /= boxCollider.size.y;
                    textureCoord += new Vector2(0.5f, 0.5f);

                    // Get the color of the sprite at the contact point
                    SpriteRenderer spriteRenderer = hit.collider.gameObject.GetComponent<SpriteRenderer>();

                    if (spriteRenderer != null)
                    {
                        Color color = spriteRenderer.sprite.texture.GetPixelBilinear(
                            textureCoord.x, textureCoord.y);

                        // Do something with the color
                        Debug.Log("Color picked: " + color);
                    }
                }
            }
        }
    }
}
