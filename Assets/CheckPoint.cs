using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Lunarsoft
{
    public class CheckPoint : MonoBehaviour
    {

        public GameObject activeObject;
        public bool checkpointActive;

        public BoxCollider2D checkpointCollider;
        public Animator animator;

        public Transform spawnLocation;
        public GameObject playerPrefab;

        public CinemachineVirtualCamera virtualCamera;

        public HerosJourneyStep herosJourneyStep;

        [SerializeField] private string sceneToLoad;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            checkpointCollider = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Activate()
        {
            if (checkpointActive == false)
            {
                animator.SetTrigger("Activated");
                checkpointActive = true;
                ScoreManager.instance.AddPoints(1000);
                ScoreManager.instance.UpdateProgress(herosJourneyStep);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Activate();
            }
        }

        public GameObject SpawnPlayer()
        {
            GameObject playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);

            virtualCamera.Follow = playerInstance.transform;
            virtualCamera.LookAt = playerInstance.transform;

            return playerInstance;
        }
    }
}
