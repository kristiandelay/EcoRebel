using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class FindClosestEnemy : MonoBehaviour
    {
        [SerializeField]
        private LayerMask enemyLayer;

        public float detectionRange = 10f;
        public Color closestLineColor = Color.green;
        public Color inRangeLineColor = Color.yellow;
        public Color detectionRangeColor = Color.blue;

        public GameObject closestEnemy;
        private List<GameObject> enemiesInRange;

        void Update()
        {
            FindEnemies();
            DrawLinesToEnemies();
        }

        private void FindEnemies()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
            closestEnemy = null;
            float closestDistance = Mathf.Infinity;
            enemiesInRange = new List<GameObject>();

            foreach (Collider2D collider in colliders)
            {
                GameObject enemy = collider.gameObject;
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                enemiesInRange.Add(enemy);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        private void DrawLinesToEnemies()
        {
            if (closestEnemy != null)
            {
                Debug.DrawLine(transform.position, closestEnemy.transform.position, closestLineColor);
            }

            foreach (GameObject enemy in enemiesInRange)
            {
                if (enemy != closestEnemy)
                {
                    Debug.DrawLine(transform.position, enemy.transform.position, inRangeLineColor);
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = detectionRangeColor;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }

}