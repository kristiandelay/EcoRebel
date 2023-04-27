using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Lunarsoft
{
    public abstract class BaseController : MonoBehaviour
    {
        [SerializeField] protected float speed = 5f;
        public Animator animator;

        [SerializeField] public BoxCollider2D playerCollider;
        [SerializeField] public PolygonCollider2D bounds;
        [SerializeField] public Transform root;

        public Transform mountTransform;
        public Transform mountSpawnTransform;
        [SerializeField] public bool isMounted = true;

        public float horizontalInput = 0;
        public float verticalInput = 0;

        public bool facingRight = true;

        public Vector3 originalScale;

        public CharacterStats characterStats;
        public Rigidbody2D rb;


        public float maxHealth = 0;
        public float currentHealth = 0;
        public float currentSpeed = 0;


        virtual protected void Awake() { }

        virtual protected void Start()
        {
            animator = GetComponent<Animator>();
            originalScale = transform.localScale;
            rb = GetComponent<Rigidbody2D>();


            if (characterStats != null)
            {
                maxHealth = currentHealth = characterStats.Health.GetValueAtLevel(characterStats.level);
                currentSpeed = characterStats.Speed.GetValueAtLevel(characterStats.level);
            }

        }

        abstract protected void Update();

        public virtual void Move(Vector2 direction)
        {
            Vector3 newPosition = transform.position + (Vector3)direction * currentSpeed * Time.deltaTime;

            // If the player's new position is inside the bounds, move the player to the new position
            if (IsInsideBounds(newPosition))
            {
                transform.position = newPosition;
                UpdateAnimator(direction);
            }
            else
            {
                // If the player's new position is outside the bounds, move the player to the closest valid position inside the bounds
                Vector3 clampedPosition = ClampPointToPolygonCollider(newPosition, bounds);
                transform.position = clampedPosition;
                UpdateAnimator(direction);
            }
        }

        protected bool IsInsideBounds(Vector3 position)
        {
            return bounds.OverlapPoint(position);
        }

        private Vector2 ClampPointToPolygonCollider(Vector2 point, PolygonCollider2D polygonCollider)
        {
            Vector2 closestPoint = polygonCollider.ClosestPoint(point);

            if (polygonCollider.OverlapPoint(closestPoint))
            {
                return closestPoint;
            }
            else
            {
                return polygonCollider.ClosestPoint(point);
            }
        }

        virtual public void UpdateAnimator(Vector2 direction)
        {
            if (animator != null)
            {
                animator.SetFloat("Speed", direction.magnitude);
            }
        }

        public virtual void ApplyKnockback(Vector2 direction, float force, float duration = 0.5f)
        {
            CinemachineShake.Instance.ShakeCamera(5f, .1f);


            Vector3 knockbackForce = direction.normalized * force;
            Vector3 targetPosition = transform.position + knockbackForce;

            // Check if the target position is inside the polygon collider
            PolygonCollider2D polygonCollider = bounds;
            if (polygonCollider != null)
            {
                // Cast a ray from the current position to the target position
                Vector3 rayDirection = (targetPosition - transform.position).normalized;
                float rayDistance = (targetPosition - transform.position).magnitude;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayDistance, LayerMask.GetMask("Obstacle"));

                // If the raycast intersects with the polygon collider, move the object to the target position
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    if (polygonCollider.OverlapPoint(targetPosition))
                    {
                        // Target position is inside the polygon collider, move the object to the target position
                        targetPosition = hit.point;
                    }
                    else
                    {
                        // Target position is outside the polygon collider, find the intersection point and move the object there
                        Vector2[] points = polygonCollider.points;
                        Vector2 closestPoint = Vector2.zero;
                        float closestDistance = float.MaxValue;

                        // Find the closest point on the polygon collider to the intersection point
                        for (int i = 0; i < points.Length; i++)
                        {
                            Vector3 point = transform.TransformPoint(points[i]);
                            float distance = Vector3.Distance(hit.point, point);
                            if (distance < closestDistance)
                            {
                                closestPoint = point;
                                closestDistance = distance;
                            }
                        }

                        targetPosition = closestPoint;
                    }
                }
            }

            // Start a coroutine to move the object over time
            StartCoroutine(MoveTowardsPosition(targetPosition, duration));
        }

        private IEnumerator MoveTowardsPosition(Vector3 targetPosition, float duration)
        {
            float timeElapsed = 0f;
            Vector3 startPosition = transform.position;

            while (timeElapsed < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure that the object reaches the target position
            transform.position = targetPosition;
        }


        abstract public void TakeDamage(float damage, string animationTrigger = "Hit");
        abstract public void Die();

    }
}