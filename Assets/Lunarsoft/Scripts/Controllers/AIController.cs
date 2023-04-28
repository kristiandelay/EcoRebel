using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class AIController : BaseController
    {
        [SerializeField] private Vector2 direction = Vector2.right;

        public AIStateBase currentState;

        public delegate void EnemyDeath(GameObject enemy);
        public static event EnemyDeath OnEnemyDeath;

        protected override void Awake()
        {
            bounds = FindAnyObjectByType<LevelBounds>()?.GetComponent<PolygonCollider2D>();

            if (GetComponent<AIStatePatrol>() == null)
            {
                gameObject.AddComponent<AIStatePatrol>();
            }

            if (GetComponent<AIStatePursue>() == null)
            {
                gameObject.AddComponent<AIStatePursue>();
            }

            if (GetComponent<AIStateAttack>() == null)
            {
                gameObject.AddComponent<AIStateAttack>();
            }
            SetState<AIStatePatrol>();
        }

        public void SetState<T>() where T : AIStateBase
        {
            currentState?.ExitState();
            currentState = GetComponent<T>();
            currentState?.EnterState();
        }


        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            currentState?.UpdateState();

            if(currentHealth < 0)
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
            OnEnemyDeath?.Invoke(gameObject);
            ScoreManager.instance.AddKill();

            Destroy(gameObject, .2f);
        }

      

    }
}