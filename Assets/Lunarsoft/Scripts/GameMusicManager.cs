using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Audio;

namespace Lunarsoft
{
    public class GameMusicManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] songs;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float fadeDuration = 1.0f;
        [SerializeField] private float volume = 0.5f;

        private int currentSongIndex = -1;
        private static GameMusicManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (songs.Length > 0)
            {
                audioSource.volume = 0f;
                PlayRandomSong();
            }
        }

        private void PlayRandomSong()
        {
            int nextSongIndex;
            do
            {
                nextSongIndex = Random.Range(0, songs.Length);
            } while (nextSongIndex == currentSongIndex);

            currentSongIndex = nextSongIndex;

            StartCoroutine(FadeInOutAndPlay(songs[currentSongIndex]));
        }

        private IEnumerator FadeInOutAndPlay(AudioClip clip)
        {
            yield return StartCoroutine(FadeOut(fadeDuration));

            audioSource.clip = clip;
            audioSource.Play();

            yield return StartCoroutine(FadeIn(fadeDuration));

            yield return new WaitForSeconds(clip.length - fadeDuration);

            PlayRandomSong();
        }

        private IEnumerator FadeIn(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                audioSource.volume = Mathf.Lerp(0f, volume, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            audioSource.volume = volume;
        }

        private IEnumerator FadeOut(float duration)
        {
            float startVolume = audioSource.volume;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            audioSource.volume = 0f;
        }
    }
}
