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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && other.GetComponent<BoxCollider2D>() != null)
            {
                // Add points to player's score
                ScoreManager.instance.AddPoints(points);

                if (collectSounds.Length > 0)
                {
                    // Play random collect sound at random volume
                    int randomIndex = Random.Range(0, collectSounds.Length);
                    AudioClip randomClip = collectSounds[randomIndex];
                    float randomVolume = Random.Range(0.8f, 1.2f);
                    AudioSource.PlayClipAtPoint(randomClip, transform.position, randomVolume);
                }
                // Destroy collectible game object
                Destroy(gameObject);
            }
        }
    }
}
