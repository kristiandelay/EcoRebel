using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class AIStateAttack : AIStateBase
    {
        FollowTarget followTarget;
        LightAttackAction action;

        protected override void OnEnterState()
        {
            action = GetComponent<LightAttackAction>();
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

            if(action.isAttacking == false)
            {
                action.triggerAttack = true;
            }
            Debug.Log($"AIStateAttack followTarget.distanceFromTarget: {followTarget.distanceFromTarget}");

        }
    }
}