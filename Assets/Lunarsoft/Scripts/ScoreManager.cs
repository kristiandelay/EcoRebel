using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Lunarsoft
{

    public enum HerosJourneyStep
    {
        // World Checkpoints
        CowExit,

        // Departure (Separation)
        OrdinaryWorld,
        CallToAdventure,
        RefusalOfTheCall,
        MeetingTheMentor,
        CrossingTheThreshold,

        // Initiation
        TestsAlliesEnemies,
        ApproachToTheInmostCave,
        TheOrdeal,
        RewardSeizingTheSword,

        // Return
        TheRoadBack,
        Resurrection,
        ReturnWithTheElixir
    }

    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        public PlayerController playerController;
        public int score = 0;
        public Text scoreText;

        public int gold = 0;
        public CharacterStats stats;

        private int kills = 0;
        private int deaths = 0;
        public int foundShovel = 0;

        public HerosJourneyStep currentProgress;
        public CheckPoint currentCheckPoint;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                LoadData(); 
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }


        private void Start()
        {
            UpdateScoreText();
            StartCoroutine(SaveDataCoroutine());

            SpawnAtCurrentCheckPoint();
        }

        private void Update()
        {
            if(playerController == null)
            {
                SpawnAtCurrentCheckPoint();
            }
        }

        public void SpawnAtCurrentCheckPoint()
        {
            CheckPoint[] checkPoints = FindObjectsOfType<CheckPoint>();

            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.herosJourneyStep == currentProgress)
                {
                    currentCheckPoint = checkPoint;
                    Debug.Log("Found matching CheckPoint with herosJourneyStep: " + currentProgress);
                    GameObject player = currentCheckPoint.SpawnPlayer();
                    playerController = player.GetComponent<PlayerController>();
                    if(foundShovel > 0)
                    {
                        playerController.magicShovel.SetActive(true);
                        playerController.GetComponent<LightAttackAction>().enabled = true;
                    }
                    else
                    {
                        playerController.magicShovel.SetActive(false);
                        playerController.GetComponent<LightAttackAction>().enabled = false;
                    }

                    break;
                }
            }

            if (currentCheckPoint == null)
            {
                Debug.Log("No matching CheckPoint found for herosJourneyStep: " + currentProgress);
            }
        }

        public void UpdateProgress(HerosJourneyStep step)
        {
            Debug.Log("Checkoint reached " + step);
            currentProgress = step;
            SaveData();
        }


        public void PickedUpMagicShovel()
        {
            Debug.Log("PickedUpMagicShovel");
            foundShovel = 1;
            playerController.magicShovel.SetActive(true);
            playerController.GetComponent<LightAttackAction>().enabled = true;

            SaveData();
        }

        public void AddKill()
        {
            kills++;
        }

        public void AddDeath()
        {
            deaths++;
        }

        public void AddPoints(int pointsToAdd)
        {
            score += pointsToAdd;
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            Debug.Log("Score: " + score.ToString());
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score.ToString();
            }
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt("Kills", kills);
            PlayerPrefs.SetInt("Deaths", deaths);
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.SetInt("Gold", gold);
            PlayerPrefs.SetInt("FoundShovel", foundShovel);

            
            string statsJson = JsonUtility.ToJson(stats);
            PlayerPrefs.SetString("CharacterStats", statsJson);
            PlayerPrefs.SetInt("CurrentProgress", (int)currentProgress);

            PlayerPrefs.Save();
        }

        public void LoadData()
        {
            score = PlayerPrefs.GetInt("Score", 0);
            gold = PlayerPrefs.GetInt("Gold", 0);
            kills = PlayerPrefs.GetInt("Kills", 0); 
            deaths = PlayerPrefs.GetInt("Deaths", 0);
            foundShovel = PlayerPrefs.GetInt("FoundShovel", 0);

            Debug.Log("Score: " + score.ToString());
            Debug.Log("gold: " + gold.ToString());
            Debug.Log("kills: " + kills.ToString());
            Debug.Log("deaths: " + deaths.ToString());
            Debug.Log("foundShovel: " + foundShovel.ToString());

            string statsJson = PlayerPrefs.GetString("CharacterStats", "");
            if (!string.IsNullOrEmpty(statsJson))
            {
                stats = JsonUtility.FromJson<CharacterStats>(statsJson);
            }

            currentProgress = (HerosJourneyStep)PlayerPrefs.GetInt("CurrentProgress", 0);

            Debug.Log("currentProgress: " + currentProgress);
        }

        private IEnumerator SaveDataCoroutine()
        {
            while (true)
            {
                SaveData();
                yield return new WaitForSeconds(10f); // Wait for 10 seconds before saving again
            }
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void OnDisable()
        {
            SaveData();
        }
    }
}
