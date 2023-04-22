using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class BasePlayerController : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 5f;
        [SerializeField] protected float jumpForce = 10f;
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] protected Transform feetTransform;
        [SerializeField] protected Animator animator;
        [SerializeField] protected float feetRadius = 0.4f;
        [SerializeField] protected SpriteRenderer spriteRenderer;

        [SerializeField] protected Collider2D attackHitbox;
        [SerializeField] protected float attackDuration = 0.3f;
        [SerializeField] protected string attackInput = "Fire1";

        protected Rigidbody2D rb2d;
        protected bool isGrounded;
        protected bool isAttacking;
        protected float attackTimer;

        protected Vector3 initialScale;

        protected virtual void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            attackHitbox.enabled = false;
            initialScale = transform.localScale;
        }

        protected virtual void Update()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            rb2d.velocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);

            if (moveHorizontal != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(moveHorizontal) * initialScale.x, initialScale.y, initialScale.z);
            }

            isGrounded = Physics2D.OverlapCircle(feetTransform.position, feetRadius, groundLayer);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }

            if (Input.GetButtonDown(attackInput) && !isAttacking)
            {
                PerformAttack();
            }

            if (isAttacking)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    attackHitbox.enabled = false;
                    isAttacking = false;
                }
            }

            animator.SetBool("Grounded", isGrounded);
            animator.SetFloat("Y_Speed", rb2d.velocity.y);
            animator.SetFloat("X_Speed_Abs", Mathf.Abs(rb2d.velocity.x));
            animator.SetBool("Attacking", isAttacking);
        }

        protected void PerformAttack()
        {
            isAttacking = true;
            attackHitbox.enabled = true;
            attackTimer = attackDuration;
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (feetTransform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(feetTransform.position, feetRadius);
            }
        }
#endif
    }
}
