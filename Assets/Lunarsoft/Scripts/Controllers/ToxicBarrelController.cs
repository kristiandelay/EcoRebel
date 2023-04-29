using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

namespace Lunarsoft
{
    public class ToxicBarrelController : AIController
    {
        public List<GameObject> collectablePrefabs;

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

            animator.SetTrigger(animationTrigger);

            if (currentHealth <= 0)
            {
                // Handle character death
                Die();
            }
        }

        public override void Die()
        {
            ScoreManager.instance.AddRemovedBarrel();

            // 60% chance to spawn a collectable
            if (Random.Range(0f, 1f) <= 0.6f && collectablePrefabs.Count > 0)
            {
                int randomIndex = Random.Range(0, collectablePrefabs.Count);
                GameObject randomCollectablePrefab = collectablePrefabs[randomIndex];
                Instantiate(randomCollectablePrefab, transform.position, Quaternion.identity);
            }

            // Destroy the barrel
            Destroy(gameObject);
        }
    }
}
