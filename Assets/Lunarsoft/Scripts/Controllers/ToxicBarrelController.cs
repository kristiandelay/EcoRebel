using UnityEngine;
using System.Collections;

namespace Lunarsoft
{
    public class ToxicBarrelController : AIController
    {

        public override void Die()
        {
            ScoreManager.instance.AddRemovedBarrel();
            Destroy(gameObject, .2f);
        }

    }
}