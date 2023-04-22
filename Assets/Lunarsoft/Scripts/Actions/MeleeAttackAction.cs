using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class MeleeAttackAction : BaseAction
    {
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private Transform attackPoint;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

        }

        public void PerformAttack()
        {
            // Play attack animation if needed
             controller.animator.SetTrigger("LightAttack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<AIController>();
                if (enemy != null)
                {
                    Debug.Log("hit that bitch ass enemy");
                }

                // Apply damage or any other effect to the enemy
                //enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
                return;

            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
