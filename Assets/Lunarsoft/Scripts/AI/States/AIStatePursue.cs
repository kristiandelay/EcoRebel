using System.Collections;
using System.Collections.Generic;
using PolyNav;
using UnityEngine;

namespace Lunarsoft
{
    public class AIStatePursue : AIStateBase
    {
        protected FollowTarget followTarget;

        protected override void OnEnterState()
        {
            followTarget = GetComponent<FollowTarget>();
            if(followTarget)
            {
                followTarget.enabled = true;
                followTarget.target = findClosestEnemy?.closestEnemy?.transform;
            }
        }

        protected override void OnExitState()
        {
            if (followTarget)
            {
                followTarget.enabled = false;
            }
        }

        protected override void OnUpdateState()
        {

            if(findClosestEnemy.closestEnemy == null)
            {
                controller.SetState<AIStatePatrol>();
            }


            if (followTarget.target != null && followTarget.distanceFromTarget > 0 && followTarget.distanceFromTarget < agent.stoppingDistance)
            {
                controller.SetState<AIStateAttack>();
            }
            //Debug.Log($"followTarget.distanceFromTarget: {followTarget.distanceFromTarget}");
        }
    }
}