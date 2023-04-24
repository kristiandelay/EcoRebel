using UnityEngine;

namespace Lunarsoft
{
    public class PatrolPoint : MonoBehaviour
    {
        [SerializeField] private float radius = 0.5f; // The radius of the debug circle
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                spriteRenderer.enabled = false;
            }
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
            {
                spriteRenderer.enabled = true;
            }
        }
    }
}
