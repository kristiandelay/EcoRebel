using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    [RequireComponent(typeof(AudioSource))]
    public class FootStepParticles : MonoBehaviour
    {
        [Tooltip("The particle effect to spawn at the feet.")]
        public GameObject footstepVFX;
        [Tooltip("The transform of the left foot.")]
        public Transform leftFoot;
        [Tooltip("The transform of the right foot.")]
        public Transform rightFoot;
        [Tooltip("The array of footstep sounds to play when the footstep effect is triggered.")]
        public AudioClip[] footstepSounds;
        [Tooltip("The minimum and maximum volume of the footstep sound.")]
        public Vector2 soundVolumeRange = new Vector2(0.5f, 0.8f);

        private AudioSource audioSource;

        protected void Start()
        {
            audioSource = GetComponent<AudioSource>();

            audioSource.spatialBlend = 1.0f; // 1.0f for 3D sound, 0.0f for 2D sound
            audioSource.minDistance = 1.0f; // The distance where the sound volume starts attenuating
            audioSource.maxDistance = 50.0f; // The distance where the sound volume reaches zero

        }

        public void TriggerLeftFootEffect()
        {
            Instantiate(footstepVFX, leftFoot.position, Quaternion.identity);
            PlayRandomFootstepSound();
        }

        public void TriggerRightFootEffect()
        {
            Instantiate(footstepVFX, rightFoot.position, Quaternion.identity);
            PlayRandomFootstepSound();
        }

        private void PlayRandomFootstepSound()
        {
            if (audioSource != null && footstepSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, footstepSounds.Length);
                audioSource.clip = footstepSounds[randomIndex];
                audioSource.volume = Random.Range(soundVolumeRange.x, soundVolumeRange.y);
                audioSource.Play();
            }
        }
    }
}
