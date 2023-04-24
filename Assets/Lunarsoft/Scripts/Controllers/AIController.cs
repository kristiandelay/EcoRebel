using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class AIController : BaseController
    {
        [SerializeField] private Vector2 direction = Vector2.right;

        public AIStateBase currentState;

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

        public override void Die()
        {

        }

        public override void TakeDamage(float damage, string animationTrigger = "Hit")
        {
            animator.SetTrigger(animationTrigger);
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            currentState?.UpdateState();
        }
    }
}