using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class AIStateAttack : AIStateBase
    {
        FollowTarget followTarget;

        protected override void OnEnterState()
        {
            followTarget = GetComponent<FollowTarget>();
            if (followTarget)
            {
                followTarget.enabled = true;
                followTarget.target = findClosestEnemy.closestEnemy.transform;
            }

        }

        protected override void OnExitState()
        {
        }

        protected override void OnUpdateState()
        {
            if (followTarget.target != null && followTarget.distanceFromTarget > agent.stoppingDistance)
            {
                controller.SetState<AIStatePatrol>();
            }
            Debug.Log($"AIStateAttack followTarget.distanceFromTarget: {followTarget.distanceFromTarget}");

        }
    }
}