using UnityEngine;

namespace Lunarsoft
{
    public class Collectible : MonoBehaviour
    {
        public enum CollectibleType
        {
            Coin,
            PowerUp
        }

        public CollectibleType type;
        public int points = 1;
        public Sprite sprite;
        public AudioClip[] collectSounds;
        public float minVolume = 0.1f; 
        public float maxVolume = 0.2f; 

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ScoreManager.instance.AddPoints(points);

                if (collectSounds.Length > 0)
                {
                    int randomIndex = Random.Range(0, collectSounds.Length);
                    AudioClip randomClip = collectSounds[randomIndex];
                    float randomVolume = Random.Range(minVolume, maxVolume); 
                    SoundManager.Instance.Play2D(randomClip, randomVolume);
                }

                Destroy(gameObject);
            }
        }
    }
}
