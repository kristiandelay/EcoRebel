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
        public Vector2 soundVolumeRange = new Vector2(0.2f, 0.8f);

        private AudioSource audioSource;

        protected void Start()
        {
            audioSource = GetComponent<AudioSource>();
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
