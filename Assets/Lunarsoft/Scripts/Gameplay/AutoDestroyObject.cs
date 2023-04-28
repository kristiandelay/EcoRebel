using UnityEngine;
using System.Collections;

namespace Lunarsoft
{
    public class AutoDestroyObject : MonoBehaviour
    {
        // The ParticleSystem to be destroyed
        [Tooltip("The ParticleSystem to be destroyed.")]
        public ParticleSystem targetParticleSystem;

        // The duration of the fade effect
        [Tooltip("The duration of the fade effect.")]
        public float fadeDuration = 1.0f;

        // Private variables to keep track of elapsed time and fading status
        private float elapsedTime = 0;
        private bool isFading = true;

        // Private variables to store the original and faded gradients
        private Gradient originalGradient;
        private Gradient fadedGradient;

        // Start is called before the first frame update
        private void Start()
        {
            // If no ParticleSystem is specified, use the one attached to the same GameObject
            if (targetParticleSystem == null)
            {
                targetParticleSystem = GetComponent<ParticleSystem>();
            }

            if (targetParticleSystem != null)
            {
                // Enable the color module of the ParticleSystem
                var colorModule = targetParticleSystem.colorOverLifetime;
                colorModule.enabled = true;

                // Store the original color gradient
                originalGradient = colorModule.color.gradient;

                // Create a new gradient with faded colors
                GradientAlphaKey[] alphaKeys = new GradientAlphaKey[originalGradient.alphaKeys.Length];
                for (int i = 0; i < originalGradient.alphaKeys.Length; i++)
                {
                    alphaKeys[i] = new GradientAlphaKey(0, originalGradient.alphaKeys[i].time);
                }
                fadedGradient = new Gradient();
                fadedGradient.SetKeys(originalGradient.colorKeys, alphaKeys);
            }
            else
            {
                Destroy(gameObject, fadeDuration);

            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (targetParticleSystem != null)
            {
                // If the ParticleSystem is still fading
                if (isFading)
                {
                    // Increment the elapsed time
                    elapsedTime += Time.deltaTime;

                    // Calculate the current time ratio
                    float t = elapsedTime / fadeDuration;

                    // If the fade effect is complete
                    if (t >= 1)
                    {
                        t = 1;
                        isFading = false;
                    }

                    // Lerp between the original and faded gradient over time
                    var colorModule = targetParticleSystem.colorOverLifetime;
                    Gradient currentGradient = new Gradient();
                    currentGradient.SetKeys(originalGradient.colorKeys, LerpAlphaKeys(originalGradient.alphaKeys, fadedGradient.alphaKeys, t));
                    colorModule.color = currentGradient;

                    // If fading is complete, destroy the object
                    if (!isFading)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        // Helper function to interpolate between two arrays of GradientAlphaKey
        private GradientAlphaKey[] LerpAlphaKeys(GradientAlphaKey[] a, GradientAlphaKey[] b, float t)
        {
            GradientAlphaKey[] result = new GradientAlphaKey[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                float alpha = Mathf.Lerp(a[i].alpha, b[i].alpha, t);
                result[i] = new GradientAlphaKey(alpha, a[i].time);
            }
            return result;
        }
    }
}
