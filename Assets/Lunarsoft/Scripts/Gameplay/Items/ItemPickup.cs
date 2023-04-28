using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class ItemPickup : MonoBehaviour
    {
        public string playerTag = "Player";

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                PickupItem();
                Destroy(gameObject);
            }
        }

        protected virtual void Update()
        {
           
        }

        protected virtual void PickupItem()
        {
            Debug.Log("Item picked up!");
        }
    }
}
