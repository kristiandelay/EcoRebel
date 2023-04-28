using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Lunarsoft
{
    public class SplashScreen : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField] public int nextSceneIndex;

        [SerializeField] private float delayTime = 4f;
        [SerializeField] private float fadeDuration = 1f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(delayTime);

            float currentTime = 0f;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime;
                canvasGroup.alpha = 1f - currentTime / fadeDuration;
                yield return null;
            }

            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
