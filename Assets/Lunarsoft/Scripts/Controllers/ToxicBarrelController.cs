using UnityEngine;
using System.Collections;

namespace Lunarsoft
{
    public class ToxicBarrelController : AIController
    {

        protected override void Update()
        {
            if (isDead == false && currentHealth < 0)
            {
                Die();
            }
        }

        public override void TakeDamage(float damage, string animationTrigger = "Hit")
        {
            currentHealth -= damage;

            // Play any damage taken animations or sounds here
            animator.SetTrigger(animationTrigger);

            if (currentHealth <= 0)
            {
                // Handle character death

                Die();
            }
        }

        public override void Die()
        {
            Debug.Log("AddRemovedBarrel DIE here mfer");
            ScoreManager.instance.AddRemovedBarrel();
            Destroy(gameObject);
        }

    }
}