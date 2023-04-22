using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class MagicAction : BaseAction
    {
        [SerializeField] private GameObject magicBoltPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float boltSpeed = 50f;
        [SerializeField] private float destroyAfter = 5f;

        [SerializeField] private GameObject[] magicspawnVFX; 
        [SerializeField] private AudioClip[] magicSpawnSound;

        private AudioSource audioSource;

        private float nextFireTime = 0f;

        protected override void Start()
        {
            base.Start();
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                gameObject.AddComponent<AudioSource>();
                audioSource = GetComponent<AudioSource>();
            }
        }

        protected override void Update()
        {
            base.Update();

            if (Time.time >= nextFireTime && Input.GetKeyDown(KeyCode.U))
            {
                CastMagicBolt();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }

        private void CastMagicBolt()
        {
            controller.animator.SetTrigger("CastMagic");

            // Instantiate random magicspawnVFX prefab and play random magic spawn sound
            int vfxIndex = Random.Range(0, magicspawnVFX.Length);
            int soundIndex = Random.Range(0, magicSpawnSound.Length);
            GameObject spawnedVFX = Instantiate(magicspawnVFX[vfxIndex], firePoint.position, firePoint.rotation);
            audioSource.PlayOneShot(magicSpawnSound[soundIndex]);

            GameObject magicBolt = Instantiate(magicBoltPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = magicBolt.GetComponent<Rigidbody2D>();
            rb.velocity = controller.facingRight ? new Vector2(boltSpeed, 0) : new Vector2(-boltSpeed, 0);

            Destroy(spawnedVFX, destroyAfter);
            Destroy(magicBolt, destroyAfter);
        }
    }
}
