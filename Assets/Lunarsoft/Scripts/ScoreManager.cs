using UnityEngine;
using UnityEngine.UI;

namespace Lunarsoft
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        public int score = 0;
        public Text scoreText;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            UpdateScoreText();
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
    }
}
