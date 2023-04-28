using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class MagicShovelPickup : ItemPickup
    {
        protected override void Update()
        {
            if(ScoreManager.instance.foundShovel > 0)
            {
                Destroy(gameObject);
            }
        }

        protected override void PickupItem()
        {
            Debug.Log("Magic Shovel Picked up!");
            ScoreManager.instance.PickedUpMagicShovel();
        }
    }
}
