using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class ItemPickup : MonoBehaviour
    {
        public string playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                PickupItem();
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if(ScoreManager.instance.foundShovel > 0)
            {
                Destroy(gameObject);
            }
        }

        private void PickupItem()
        {
            Debug.Log("Item picked up!");
            ScoreManager.instance.PickedUpMagicShovel();
        }
    }
}
