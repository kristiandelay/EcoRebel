using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class CowAIController : AIController
    {
        public List<GameObject> collectablePrefabs;


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

            // 60% chance to spawn a collectable
            if (Random.Range(0f, 1f) <= 0.6f && collectablePrefabs.Count > 0)
            {
                int randomIndex = Random.Range(0, collectablePrefabs.Count);
                GameObject randomCollectablePrefab = collectablePrefabs[randomIndex];
                Instantiate(randomCollectablePrefab, transform.position, Quaternion.identity);
            }

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