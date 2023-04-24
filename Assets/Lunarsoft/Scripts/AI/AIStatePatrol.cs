using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class AIStatePatrol : AIStateBase
    {

        protected override void OnEnterState()
        {
            GetComponent<PatrolRandomWaypoints>().enabled = true;
        }

        protected override void OnUpdateState()
        {
            if(findClosestEnemy.closestEnemy != null)
            {
                controller.SetState<AIStatePursue>();
            }
        }

        protected override void OnExitState()
        {
            GetComponent<PatrolRandomWaypoints>().enabled = false;
        }
    }

}