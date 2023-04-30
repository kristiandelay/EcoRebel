using UnityEngine;

namespace Lunarsoft
{
    // Example class representing objects that can be culled
    [RequireComponent(typeof(BoxCollider2D))]
    public class CullableObject : MonoBehaviour
    {
        private BoxCollider2D boxCollider;

        private void Awake()
        {
            // Get the BoxCollider2D component
            boxCollider = GetComponent<BoxCollider2D>();

            // Set the BoxCollider2D as a trigger
            boxCollider.isTrigger = true;
        }

        // Called when the object's trigger collider enters another collider
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the other collider is attached to the camera
            if (other.gameObject.CompareTag("MainCamera"))
            {
                // Set the object as visible
                SetVisible(true);
            }
        }

        // Called when the object's trigger collider exits another collider
        private void OnTriggerExit2D(Collider2D other)
        {
            // Check if the other collider is attached to the camera
            if (other.gameObject.CompareTag("MainCamera"))
            {
                // Set the object as not visible
                SetVisible(false);
            }
        }

        // Set the visibility of the object
        private void SetVisible(bool isVisible)
        {
            // Enable or disable the object's renderer based on visibility
            GetComponent<Renderer>().enabled = isVisible;
        }
    }
}
