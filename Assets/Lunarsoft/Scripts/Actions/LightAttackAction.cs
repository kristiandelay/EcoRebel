using UnityEngine;
using System.Collections.Generic;

namespace Lunarsoft
{
    public class LightAttackAction : BaseAction
    {
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private Transform attackPoint;
        [SerializeField] public float attackCooldown = 1f;
        [SerializeField] private GameObject hitEffectPrefab;

        [SerializeField] private AudioClip[] attackSounds;
        [SerializeField] private AudioClip[] attackImpactSounds;
        [SerializeField] private AudioSource audioSource;


        public bool isAI = false;
        public bool triggerAttack = false;

        public float currentAttackCooldown = 0f;
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

            if(GetComponent<AIController>() != null)
            {
                isAI = true;
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

            if (isAI == false)
            {
                if (currentAttackCooldown <= 0 && Input.GetKeyDown(KeyCode.J))
                {
                    PerformAttack();
                    currentAttackCooldown = calculatedCooldown;
                }
            } else
            {
                if (currentAttackCooldown <= 0 && triggerAttack)
                {
                    PerformAttack();
                    currentAttackCooldown = calculatedCooldown;
                }
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

                BaseController baseController = enemy.GetComponent<BaseController>();
                if (baseController != null)
                {
                    float damage = controller.characterStats.AttackDamage.GetValueAtLevel(controller.characterStats.level);

                    if (hitEffectPrefab != null)
                    {
                        GameObject hitEffectInstance = Instantiate(hitEffectPrefab, attackPoint.position, Quaternion.identity);
                        hitEffectInstance.transform.localScale *= Mathf.Sign(controller.transform.localScale.x);
                    }

                    bool playerOnLeftSide = (baseController.root.transform.position.x - controller.transform.position.x) > 0;

                    string animationHit = "Hit";
                    if (playerOnLeftSide)
                    {
                        if(baseController.facingRight == true)
                        {
                            animationHit = "HitBack";
                        }
                    }
                    else
                    {
                        if (baseController.facingRight == false)
                        {
                            animationHit = "HitBack";
                        }
                    }

                    Debug.DrawLine(controller.transform.position, baseController.root.transform.position, Color.yellow, 2f);
                    PlayRandomAttackImpactSound();
                    baseController.TakeDamage(damage, animationHit);
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
            triggerAttack = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            Gizmos.color = enableHitbox ? Color.green : Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
