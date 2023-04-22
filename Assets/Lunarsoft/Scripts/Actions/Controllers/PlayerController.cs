using UnityEngine;
using Cinemachine;
using Lunarsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System;

public class PlayerController : BaseController
{
    // The Rewired player id of this character
    public int playerId = 0;

    public CinemachineVirtualCamera virtualCamera;
    public Camera cam;

    public CharacterStats characterStats;

    public float maxHealth = 0;
    public float currentHealth = 0;
    public float currentSpeed = 0;

    private BaseApi _baseApi;

    protected override void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime

        if (characterStats != null)
        {
            maxHealth = currentHealth = characterStats.Health.GetValueAtLevel(characterStats.level);
            currentSpeed = characterStats.Speed.GetValueAtLevel(characterStats.level);
        }

        bounds = FindAnyObjectByType<LevelBounds>()?.GetComponent<PolygonCollider2D>();

        _baseApi = GetComponent<BaseApi>();
        if (_baseApi != null)
        {
            StartCoroutine(PerformGetRandomUserApiCalls());
        }

    }

   

    private IEnumerator PerformGetRandomUserApiCalls()
    {
        // GET request example
        string getEndpoint = "/";
        _baseApi.baseUrl = "https://randomuser.me/api";
        yield return _baseApi.Get(getEndpoint, response => {
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
        }, error => {
            Debug.LogError("GET Error: " + error);
        });
    }


    protected override void Update()
    { 
    }

    public override void Move(Vector2 direction)
    {
        Vector3 newPosition = transform.position + (Vector3)direction * currentSpeed * Time.deltaTime;
        if (IsInsideBounds(newPosition))
        {
            transform.position = newPosition;
            UpdateAnimator(direction);
        }
    }

    // Override the TakeDamage function to use the character's health stat
    public override void TakeDamage(float damage, string animationTrigger = "Hit")
    {
        currentHealth -= damage;

        // Play any damage taken animations or sounds here

        if (currentHealth <= 0)
        {
            // Handle character death
            Die();
        }
    }

    public override void Die()
    {
        // Implement your die logic here
    }
}
