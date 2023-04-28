using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform character;
        [SerializeField] private float spawnDistance = 5f;
        [SerializeField] private int maxSpawnedEnemies = 5;

        private List<GameObject> spawnedEnemies = new List<GameObject>();

        private void Start()
        {
            AIController.OnEnemyDeath += OnEnemyDeath;

            for (int i = 0; i < maxSpawnedEnemies; i++)
            {
                SpawnEnemy();
            }
        }

        private void OnDestroy()
        {
            AIController.OnEnemyDeath -= OnEnemyDeath;
        }

        private void SpawnEnemy()
        {
            float angle = Random.Range(0f, 360f);
            Vector2 spawnPosition = new Vector2(character.position.x + spawnDistance * Mathf.Cos(angle * Mathf.Deg2Rad),
                                                character.position.y + spawnDistance * Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(newEnemy);
            BaseController enemyController = newEnemy.GetComponent<BaseController>();

            if (enemyController != null)
            {
                //enemyController.Initialize();
            }
            else
            {
                Debug.LogError("Enemy prefab does not have a BaseController component!");
            }
        }

        private void OnEnemyDeath(GameObject deadEnemy)
        {
            spawnedEnemies.Remove(deadEnemy);
            ScoreManager.instance.AddKill();
            if (spawnedEnemies.Count < maxSpawnedEnemies)
            {
                SpawnEnemy();
            }
        }

        private void OnDrawGizmos()
        {
            if (character != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(character.position, spawnDistance);
            }
        }
    }
}