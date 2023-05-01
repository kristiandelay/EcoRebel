using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Lunarsoft
{

    public enum HerosJourneyStep
    {
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
        ReturnWithTheElixir,

        // Other World Checkpoints
        CowExit,
        SewerPipeEntrence,
        SewerPipeExit,
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
        private int toxicBarrlesRemoved = 0;
        public int foundShovel = 0;

        public HerosJourneyStep currentProgress;
        public CheckPoint currentCheckPoint;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                LoadData();
                //ClearData();
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

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene " + scene.name + " has been loaded.");
            SpawnAtCurrentCheckPoint();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            
        }

        public void SpawnAtCurrentCheckPoint()
        {
            Time.timeScale = 1f;

            CheckPoint[] checkPoints = FindObjectsOfType<CheckPoint>();
            CheckPoint lastcheckedCheckPoint = null ;
            foreach (CheckPoint checkPoint in checkPoints)
            {
                lastcheckedCheckPoint = checkPoint;
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
                if(currentProgress == HerosJourneyStep.CowExit)
                {
                    SceneManager.LoadScene(1);
                }
                else
                {
                    Debug.Log("lastcheckedCheckPoint.herosJourneyStep: " + lastcheckedCheckPoint.herosJourneyStep);
                    UpdateProgress(lastcheckedCheckPoint.herosJourneyStep);
                    Debug.Log("No matching CheckPoint found for herosJourneyStep: " + currentProgress);

                }
                Debug.Log("No matching CheckPoint found for herosJourneyStep: " + currentProgress);
            }
        }

        public void UpdateProgress(HerosJourneyStep step)
        {
            Debug.Log("Checkoint reached " + step);
            currentProgress = step;
            SaveData();
        }

        public void Respawn()
        {
            SpawnAtCurrentCheckPoint();
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
            SaveData();
        }

        public void AddDeath()
        {
            deaths++;
            SaveData();
        }

        public void AddRemovedBarrel()
        {
            Debug.Log("calling AddRemovedBarrel mfer");

            toxicBarrlesRemoved++;
            SaveData();
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

        public void ClearData()
        {
            // Reset variables to default values
            score = 0;
            gold = 0;
            kills = 0;
            deaths = 0;
            foundShovel = 0;
            toxicBarrlesRemoved = 0;
            currentProgress = HerosJourneyStep.OrdinaryWorld;
            stats = new CharacterStats(); // Assuming CharacterStats has a default constructor

            // Update UI
            UpdateScoreText();

            // Clear data from PlayerPrefs
            PlayerPrefs.DeleteKey("Kills");
            PlayerPrefs.DeleteKey("Deaths");
            PlayerPrefs.DeleteKey("Score");
            PlayerPrefs.DeleteKey("toxicBarrlesRemoved");
            PlayerPrefs.DeleteKey("Gold");
            PlayerPrefs.DeleteKey("FoundShovel");
            PlayerPrefs.DeleteKey("CharacterStats");
            PlayerPrefs.DeleteKey("CurrentProgress");

            // Save changes to PlayerPrefs
            PlayerPrefs.Save();

            // Optionally, respawn the player at the starting checkpoint
            SpawnAtCurrentCheckPoint();
        }


        public void SaveData()
        {
            PlayerPrefs.SetInt("Kills", kills);
            PlayerPrefs.SetInt("Deaths", deaths);
            PlayerPrefs.SetInt("ToxicBarrlesRemoved", toxicBarrlesRemoved);
            
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.SetInt("Gold", gold);
            PlayerPrefs.SetInt("FoundShovel", foundShovel);

            //string statsJson = JsonUtility.ToJson(stats);
            //PlayerPrefs.SetString("CharacterStats", statsJson);
            PlayerPrefs.SetInt("CurrentProgress", (int)currentProgress);

            PlayerPrefs.Save();
        }

        public void LoadData()
        {
            score = PlayerPrefs.GetInt("Score", 0);
            gold = PlayerPrefs.GetInt("Gold", 0);
            kills = PlayerPrefs.GetInt("Kills", 0); 
            deaths = PlayerPrefs.GetInt("Deaths", 0);
            toxicBarrlesRemoved = PlayerPrefs.GetInt("ToxicBarrlesRemoved", 0);
            
            foundShovel = PlayerPrefs.GetInt("FoundShovel", 0);

            Debug.Log("Score: " + score.ToString());
            Debug.Log("gold: " + gold.ToString());
            Debug.Log("kills: " + kills.ToString());
            Debug.Log("deaths: " + deaths.ToString());
            Debug.Log("toxicBarrlesRemoved: " + toxicBarrlesRemoved.ToString());
            Debug.Log("foundShovel: " + foundShovel.ToString());

            //string statsJson = PlayerPrefs.GetString("CharacterStats", "");
            //if (!string.IsNullOrEmpty(statsJson))
            //{
            //    stats = JsonUtility.FromJson<CharacterStats>(statsJson);
            //}

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
