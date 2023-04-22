using UnityEngine;
using System.Collections.Generic;

namespace Lunarsoft
{
    public class LightAttackAction : BaseAction
    {
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private GameObject hitEffectPrefab;

        [SerializeField] private AudioClip[] attackSounds;
        [SerializeField] private AudioClip[] attackImpactSounds;
        [SerializeField] private AudioSource audioSource;


        private float currentAttackCooldown = 0f;
        public bool isAttacking { get; private set; }
        public bool enableHitbox { get; private set; }

        private List<Collider2D> hitEnemies = new List<Collider2D>();

        protected override void Start()
        {
            base.Start();

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        protected override void Update()
        {
            base.Update();

            if (currentAttackCooldown > 0)
            {
                currentAttackCooldown -= Time.deltaTime;
            }

            if (enableHitbox)
            {
                checkCollision();
            }

            if (isAttacking)
            {
                return;
            }

            float currentAttackSpeed = controller.characterStats.AttackSpeed.GetValueAtLevel(controller.characterStats.level);
            float calculatedCooldown = attackCooldown / currentAttackSpeed;

            if (currentAttackCooldown <= 0 && Input.GetKeyDown(KeyCode.J))
            {
                PerformAttack();
                currentAttackCooldown = calculatedCooldown;
            }
        }

        public void PerformAttack()
        {
            PlayRandomAttackSound();
            controller.animator.SetTrigger("LightAttack");
            isAttacking = true;
            hitEnemies.Clear();
        }

        public void checkCollision()
        {
            Collider2D[] potentialHitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider2D enemy in potentialHitEnemies)
            {
                if (hitEnemies.Contains(enemy))
                {
                    continue; // Enemy has already been hit during this attack
                }

                AIController aiController = enemy.GetComponent<AIController>();
                if (aiController != null)
                {
                    float damage = controller.characterStats.AttackDamage.GetValueAtLevel(controller.characterStats.level);

                    // Instantiate hit effect prefab at attack point position with same scale signum as player
                    if (hitEffectPrefab != null)
                    {
                        GameObject hitEffectInstance = Instantiate(hitEffectPrefab, attackPoint.position, Quaternion.identity);
                        hitEffectInstance.transform.localScale *= Mathf.Sign(controller.transform.localScale.x);
                    }

                    // Calculate the dot product between the player's forward direction and the playerToEnemy vector
                    bool playerOnLeftSide = (aiController.root.transform.position.x - controller.transform.position.x) > 0;

                    string animationHit = "Hit";
                    if (playerOnLeftSide)
                    {
                        // Player is hitting the right side of the enemy
                        animationHit = "HitBack";
                    }
                    else
                    {
                        // Player is hitting the left side of the enemy
                        animationHit = "Hit";
                        // controller.animator.SetTrigger("LeftHit");
                    }

                    // Draw line between player and hit position for debugging
                    Debug.DrawLine(controller.transform.position, aiController.root.transform.position, Color.yellow, 2f);
                    PlayRandomAttackImpactSound();
                    aiController.TakeDamage(damage, animationHit);
                    hitEnemies.Add(enemy);
                }
            }
        }

        private void PlayRandomAttackSound()
        {
            if (audioSource != null && attackSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, attackSounds.Length);
                audioSource.clip = attackSounds[randomIndex];
                audioSource.Play();
            }
        }

        private void PlayRandomAttackImpactSound()
        {
            if (audioSource != null && attackImpactSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, attackImpactSounds.Length);
                audioSource.clip = attackImpactSounds[randomIndex];
                audioSource.Play();
            }
        }

        public void EnableHitbox()
        {
            enableHitbox = true;
        }

        public void DisableHitbox()
        {
            enableHitbox = false;
        }

        public void FinishedAttack()
        {
            isAttacking = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            Gizmos.color = enableHitbox ? Color.green : Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
