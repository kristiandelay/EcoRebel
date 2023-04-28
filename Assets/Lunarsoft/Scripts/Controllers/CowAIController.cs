using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class CowAIController : AIController
    {
        protected override void Awake()
        {
            bounds = FindAnyObjectByType<LevelBounds>()?.GetComponent<PolygonCollider2D>();


            if (GetComponent<AIStateCowPursue>() == null)
            {
                gameObject.AddComponent<AIStateCowPursue>();
            }

            if (GetComponent<AIStateCowAttack>() == null)
            {
                gameObject.AddComponent<AIStateCowAttack>();
            }
            SetState<AIStateCowPursue>();
        }


        public override void Die()
        {
            base.Die();
        }

        public override void TakeDamage(float damage, string animationTrigger = "Hit")
        {
            base.TakeDamage(damage, animationTrigger);
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            currentState?.UpdateState();
            if (currentHealth < 0)
            {
                Die();
            }
        }
    }
}