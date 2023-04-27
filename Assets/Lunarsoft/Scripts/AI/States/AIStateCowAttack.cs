using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class AIStateCowAttack : AIStateAttack
    {

        protected override void OnUpdateState()
        {
            if (followTarget.target != null && followTarget.distanceFromTarget > agent.stoppingDistance)
            {
                controller.SetState<AIStateCowPursue>();
            }

            if(action.isAttacking == false)
            {
                action.triggerAttack = true;
            }
            //Debug.Log($"AIStateAttack followTarget.distanceFromTarget: {followTarget.distanceFromTarget}");

        }
    }
}