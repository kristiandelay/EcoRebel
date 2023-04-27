using System.Collections;
using System.Collections.Generic;
using Lunarsoft;
using UnityEngine;

public class HS_ProjectileMover : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody2D rb;
    public GameObject[] Detached;
    public AudioClip[] hitSfx;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player OnTriggerEnter2D");
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy OnTriggerEnter2D");

            //Lock all axes movement and rotation
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            speed = 0;

            ContactPoint2D[] contacts = new ContactPoint2D[1];
            other.GetContacts(contacts);
            ContactPoint2D contact = contacts[0];

            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

            // Get the position of the projectile
            Vector3 pos = transform.position;

            // Offset the position based on the normal vector of the contact point
            pos += (Vector3)contact.normal * hitOffset;

            //Spawn hit effect on collision
            if (hit != null)
            {
                var hitInstance = Instantiate(hit, pos, rot);
                if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
                else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
                else { hitInstance.transform.LookAt(contact.point + contact.normal); }

                //Destroy hit effects depending on particle Duration time
                var hitPs = hitInstance.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    Destroy(hitInstance, hitPs.main.duration);
                }
                else
                {
                    var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitInstance, hitPsParts.main.duration);
                }
            }

            // Play a random sound from the hitSfx array
            if (hitSfx != null && hitSfx.Length > 0)
            {
                // Create a new GameObject to hold the AudioSource component
                GameObject audioSourceObject = new GameObject("HitSfx");

                // Add the AudioSource component to the new GameObject
                AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
                ProximitySound proximitySound = audioSourceObject.AddComponent<ProximitySound>();
                proximitySound.maxDistance = 150;
                proximitySound.minDistance = 1;
                proximitySound.minVolume = 0;
                proximitySound.maxVolume = 1;

                // Set the clip and play the audio
                int randomIndex = Random.Range(0, hitSfx.Length);
                audioSource.clip = hitSfx[randomIndex];
                audioSource.Play();

                // Destroy the new GameObject after the audio clip finishes playing
                Destroy(audioSourceObject, audioSource.clip.length);
            }

            //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
            foreach (var detachedPrefab in Detached)
            {
                if (detachedPrefab != null)
                {
                    detachedPrefab.transform.parent = null;
                    Destroy(detachedPrefab, 1);
                }
            }

            //Destroy projectile on collision
            Destroy(gameObject);
        }
    }
}
