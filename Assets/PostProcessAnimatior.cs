using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Lunarsoft
{
    public class PostProcessAnimatior : MonoBehaviour
    {
        public Volume volume;
        public VolumeProfile volumeProfile;
        public Tonemapping tonemapping;
        public Bloom bloom;
        public Vignette vignette;
        public FilmGrain filmGrain;
        public LensDistortion lensDistortion;
        public MotionBlur motionBlur;
        public ColorAdjustments colorAdjustments;
        public ChromaticAberration chromaticAberration;

        public float lerpSpeed = 1.0f; // Speed at which the color will oscillate

        public float maxHealth = 100.0f; // Maximum health value
        public float currentHealth = 100.0f; // Current health value

        public PlayerController player;


        // Start is called before the first frame update
        void Start()
        {

            volume = gameObject.GetComponent<Volume>();
            volume.isGlobal = true;
            volume.priority = 10;

            volumeProfile = volume.profile;
            volumeProfile.TryGet(out tonemapping);
            volumeProfile.TryGet(out bloom);
            volumeProfile.TryGet(out vignette);
            volumeProfile.TryGet(out filmGrain);
            volumeProfile.TryGet(out lensDistortion);
            volumeProfile.TryGet(out motionBlur);
            volumeProfile.TryGet(out colorAdjustments);
            volumeProfile.TryGet(out chromaticAberration);

            //colorAdjustments.active = false;
            chromaticAberration.active = false;
        }

        // Update is called once per frame
        void Update()
        {
            player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                currentHealth = player.currentHealth;

                float healthRatio = Mathf.Clamp01(currentHealth / maxHealth);
                Color lerpedColor = Color.Lerp(Color.red, Color.white, healthRatio);
                colorAdjustments.colorFilter.value = lerpedColor;
            }
        }
    }
}