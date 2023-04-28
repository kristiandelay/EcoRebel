using UnityEngine;

namespace Lunarsoft
{
    public class EnemyAI : AIController
    {
        [SerializeField] public float detectionRadius = 5f;
        [SerializeField] public float patrolSpeed = 2f;
        [SerializeField] public float chaseSpeed = 4f;
        [SerializeField] public PolygonCollider2D patrolPath;
        [SerializeField] public bool patrolCircular = true;

        private Transform player;
        private int currentPatrolIndex;
        private bool reversePatrol;

        private enum eAIState
        {
            Patrol,
            Chase
        }

        private eAIState currentAIState;

        protected override void Awake()
        {
            base.Awake();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            currentPatrolIndex = 0;
            reversePatrol = false;
            currentAIState = eAIState.Patrol;
        }

        protected override void Update()
        {
            if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
            {
                currentAIState = eAIState.Chase;
            }
            else if (currentAIState == eAIState.Chase)
            {
                currentAIState = eAIState.Patrol;
            }

            switch (currentAIState)
            {
                case eAIState.Chase:
                    ChasePlayer();
                    break;
                case eAIState.Patrol:
                    Patrol();
                    break;
            }
        }

        private void ChasePlayer()
        {
            speed = chaseSpeed;
            Vector2 direction = (player.position - transform.position).normalized;
            Move(direction);
        }

        private void Patrol()
        {
            if (patrolPath == null || patrolPath.points.Length == 0)
            {
                return;
            }

            Vector3 currentTarget = patrolPath.transform.TransformPoint(patrolPath.points[currentPatrolIndex]);
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget);

            // Set a stopping distance
            float stoppingDistance = 0.1f;

            if (distanceToTarget > stoppingDistance)
            {
                speed = patrolSpeed;
                Vector2 direction = (currentTarget - transform.position).normalized;
                Move(direction);
            }
            else
            {
                // Stop the AI
                speed = 0;
                UpdateAnimator(Vector2.zero);

                if (patrolCircular)
                {
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPath.points.Length;
                }
                else
                {
                    if (reversePatrol)
                    {
                        currentPatrolIndex--;
                        if (currentPatrolIndex < 0)
                        {
                            currentPatrolIndex = 1;
                            reversePatrol = false;
                        }
                    }
                    else
                    {
                        currentPatrolIndex++;
                        if (currentPatrolIndex >= patrolPath.points.Length)
                        {
                            currentPatrolIndex = patrolPath.points.Length - 2;
                            reversePatrol = true;
                        }
                    }
                }
            }
        }



        public override void TakeDamage(float damage, string animationTrigger = "Hit")
        {
            // Implement the logic for taking damage
        }

        public override void Die()
        {
            // Implement the logic for enemy death
        }
    }
}
