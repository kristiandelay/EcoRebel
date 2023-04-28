using UnityEngine;

namespace Lunarsoft
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; } // Singleton instance

        private AudioSource audioSource2D;
        private AudioSource audioSource3D;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Make the SoundManager persistent between scenes
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            audioSource2D = gameObject.AddComponent<AudioSource>();
            audioSource2D.spatialBlend = 0f; // 2D sound (no spatialization)

            audioSource3D = gameObject.AddComponent<AudioSource>();
            audioSource3D.spatialBlend = 1f; // 3D sound (full spatialization)
        }

        public void Play2D(AudioClip clip, float volume = 1.0f)
        {
            audioSource2D.PlayOneShot(clip, volume);
        }

        public void Play3D(AudioClip clip, Vector3 position, float volume = 1.0f)
        {
            audioSource3D.transform.position = position;
            audioSource3D.PlayOneShot(clip, volume);
        }
    }
}