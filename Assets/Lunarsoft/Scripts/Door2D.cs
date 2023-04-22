using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lunarsoft
{
    public class Door2D : MonoBehaviour
    {
        [SerializeField] private string doorID;

        [HideInInspector]
        [SerializeField] public int sceneIndex;

        [HideInInspector]
        [SerializeField] public string playerTag = "Player";

        [HideInInspector]
        [SerializeField] public string spawnPointTag = "SpawnPoint";

        private bool playerInRange = false;

        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(sceneIndex);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInRange = false;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(spawnPointTag);
            GameObject targetSpawnPoint = null;

            foreach (GameObject spawnPoint in spawnPoints)
            {
                SpawnPoint spawnPointComponent = spawnPoint.GetComponent<SpawnPoint>();
                if (spawnPointComponent && spawnPointComponent.DoorID == doorID)
                {
                    targetSpawnPoint = spawnPoint;
                    break;
                }
            }

            if (targetSpawnPoint)
            {
                GameObject player = GameObject.FindGameObjectWithTag(playerTag);
                if (player)
                {
                    player.transform.position = targetSpawnPoint.transform.position;
                }
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
