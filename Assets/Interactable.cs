using UnityEngine;
using UnityEngine.UI;

namespace Lunarsoft
{
    public class Interactable : MonoBehaviour
    {
        public GameObject canvasInteract; 

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Check if the object entering the trigger is the player
            if (collision.CompareTag("Player"))
            {
                // Show the text UI element
                canvasInteract.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // Check if the object exiting the trigger is the player
            if (collision.CompareTag("Player"))
            {
                // Hide the text UI element
                canvasInteract.SetActive(false);
            }
        }
    }
}
