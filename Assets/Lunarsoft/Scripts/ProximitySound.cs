using UnityEngine;

namespace Lunarsoft
{
    public class ProximitySound : MonoBehaviour
    {
        public float maxDistance = 10f;
        public float minDistance = 1f;
        public float minVolume = 0f;
        public float maxVolume = 1f;

        private Transform listener;
        private AudioSource audioSource;

        private void Start()
        {
            listener = Camera.main.transform;
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            float distance = Vector3.Distance(transform.position, listener.position);
            float volume = Mathf.Lerp(minVolume, maxVolume, 1f - Mathf.Clamp01(distance / (maxDistance - minDistance)));
            audioSource.volume = volume;
        }
    }
}