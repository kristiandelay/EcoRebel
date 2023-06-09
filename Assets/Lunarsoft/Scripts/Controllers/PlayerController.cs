using UnityEngine;
using Cinemachine;
using Lunarsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System;

namespace Lunarsoft
{

    public class PlayerController : BaseController
    {
        // The Rewired player id of this character
        public int playerId = 0;

        public CinemachineVirtualCamera virtualCamera;
        public Camera cam;

        private BaseApi _baseApi;

        public GameObject magicShovel;

        public PauseMenu pauseMenu;

        protected override void Start()
        {
            base.Start();

            GameObject pauseMenuObject = GameObject.Find("GameUICanvas");

            // Check if the GameObject was found
            if (pauseMenuObject != null)
            {
                // Get the PauseMenu component attached to the found GameObject
                pauseMenu = pauseMenuObject.GetComponent<PauseMenu>();

                // Check if the PauseMenu component was found
                if (pauseMenu != null)
                {
                    // Perform actions with the PauseMenu component
                    Debug.Log("PauseMenu component found!");
                }
                else
                {
                    // Handle the case where the PauseMenu component was not found
                    Debug.Log("PauseMenu component not found!");
                }
            }
            else
            {
                // Handle the case where the GameObject was not found
                Debug.Log("PauseMenuObject not found!");
            }

        }
        protected override void Awake()
        {
            // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
            base.Awake();
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            cam = FindObjectOfType<Camera>();


            bounds = FindAnyObjectByType<LevelBounds>()?.GetComponent<PolygonCollider2D>();

            _baseApi = GetComponent<BaseApi>();
            //if (_baseApi != null)
            //{
            //    StartCoroutine(PerformGetRandomUserApiCalls());
            //}
        }

        private IEnumerator PerformGetRandomUserApiCalls()
        {
            // GET request example
            string getEndpoint = "/";
            _baseApi.baseUrl = "https://randomuser.me/api";
            yield return _baseApi.Get(getEndpoint, response =>
            {
                Debug.Log("GET Response: " + response);
                try
                {
                    RandomUserApiResponse randomUserApiResponse = JsonUtility.FromJson<RandomUserApiResponse>(response);
                    Debug.Log("Deserialized user's email: " + randomUserApiResponse.results[0].email);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Deserialization Error: " + ex.Message);
                }
            }, error =>
            {
                Debug.LogError("GET Error: " + error);
            });
        }

        protected override void Update()
        {
        }

        public override void Move(Vector2 direction)
        {
            base.Move(direction);
        }

        public override void TakeDamage(float damage, string animationTrigger = "Hit")
        {
            currentHealth -= damage;

            animator.SetTrigger(animationTrigger);

            if (isDead == false && currentHealth <= 0)
            {
                Die();
            }
        }

        public override void Die()
        {
            Debug.Log("You Dead af my guy");
            ScoreManager.instance.AddDeath();
            pauseMenu.ShowDefeatMenu();
            isDead = true;
            Destroy(gameObject);
        }
    }
}