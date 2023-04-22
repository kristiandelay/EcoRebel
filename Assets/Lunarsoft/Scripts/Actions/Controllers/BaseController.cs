using System.Collections;
using System.Collections.Generic;
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


        virtual protected void Awake() { }

        virtual protected void Start()
        {
            animator = GetComponent<Animator>();
            originalScale = transform.localScale;
        }

        abstract protected void Update();

        public virtual void Move(Vector2 direction)
        {
            Vector3 newPosition = transform.position + (Vector3)direction * speed * Time.deltaTime;
            if (IsInsideBounds(newPosition))
            {
                transform.position = newPosition;
                UpdateAnimator(direction);
            }
        }

        protected bool IsInsideBounds(Vector3 position)
        {
            return bounds.OverlapPoint(position);
        }

        virtual public void UpdateAnimator(Vector2 direction)
        {
            if (animator != null)
            {
                animator.SetFloat("Speed", direction.magnitude);
            }
        }

        abstract public void TakeDamage(float damage, string animationTrigger = "Hit");
        abstract public void Die();
    }
}