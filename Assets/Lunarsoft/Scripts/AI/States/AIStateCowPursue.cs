using System.Collections;
using System.Collections.Generic;
using PolyNav;
using UnityEngine;

namespace Lunarsoft
{
    public class AIStateCowPursue : AIStatePursue
    {

        protected override void OnExitState()
        {
            if (followTarget)
            {
                followTarget.enabled = false;
            }
        }

        protected override void OnUpdateState()
        {
            if(followTarget.target == null && findClosestEnemy.closestEnemy != null )
            {
                followTarget.target = findClosestEnemy.closestEnemy.transform;
            }
            if (followTarget.target != null && followTarget.distanceFromTarget > 0 && followTarget.distanceFromTarget < agent.stoppingDistance)
            {
                controller.SetState<AIStateAttack>();
            }
            //Debug.Log($"followTarget.distanceFromTarget: {followTarget.distanceFromTarget}");
        }
    }
}