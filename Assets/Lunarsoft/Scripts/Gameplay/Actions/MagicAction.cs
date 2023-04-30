using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class MagicAction : BaseAction
    {
        [SerializeField] private GameObject magicBoltPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] public float damage = 30f;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float boltSpeed = 50f;
        [SerializeField] private float destroyAfter = 5f;

        [SerializeField] private GameObject[] magicspawnVFX;
        [SerializeField] private AudioClip[] magicSpawnSound;

        private AudioSource audioSource;
        private bool canCast = true;

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

            if (canCast && Input.GetKeyDown(KeyCode.U))
            {
                StartCoroutine(CastMagic());
            }
        }

        private IEnumerator CastMagic()
        {
            canCast = false;

            // Trigger the "CastMagic" animation
            controller.animator.SetTrigger("CastMagic");

            // Get the AnimatorStateInfo for the layer where the "CastMagic" animation is playing
            // (assuming it is in the base layer, which has an index of 0)
            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);

            // Find the length of the "CastMagic" animation
            float animationLength = 0f;
            foreach (AnimationClip clip in controller.animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "CastMagic") // Replace "CastMagic" with the actual name of the animation clip
                {
                    animationLength = clip.length;
                    break;
                }
            }

            // Wait for the duration of the animation
            yield return new WaitForSeconds(animationLength);

            // Continue with the rest of the coroutine
            yield return new WaitForSeconds(1f / fireRate);

            canCast = true;
        }


        private void CastMagicBolt()
        {
            Debug.Log("CastMagicBolt");

            // Instantiate random magicspawnVFX prefab and play random magic spawn sound
            int vfxIndex = Random.Range(0, magicspawnVFX.Length);
            int soundIndex = Random.Range(0, magicSpawnSound.Length);
            GameObject spawnedVFX = Instantiate(magicspawnVFX[vfxIndex], firePoint.position, firePoint.rotation);
            if (magicSpawnSound.Length > 0)
            {
                audioSource.PlayOneShot(magicSpawnSound[soundIndex]);
            }

            GameObject magicBolt = Instantiate(magicBoltPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = magicBolt.GetComponent<Rigidbody2D>();
            rb.velocity = controller.facingRight ? new Vector2(boltSpeed, 0) : new Vector2(-boltSpeed, 0);

            Destroy(spawnedVFX, destroyAfter);
            Destroy(magicBolt, destroyAfter);
        }
    }
}
