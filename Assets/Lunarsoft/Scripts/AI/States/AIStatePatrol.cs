using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class AIStatePatrol : AIStateBase
    {

        protected override void OnEnterState()
        {
            PatrolRandomWaypoints waypoint = GetComponent<PatrolRandomWaypoints>();
            if(waypoint != null)
            {
                waypoint.enabled = true;
            }
        }

        protected override void OnUpdateState()
        {
            if(findClosestEnemy?.closestEnemy != null)
            {
                controller.SetState<AIStatePursue>();
            }
        }

        protected override void OnExitState()
        {
            PatrolRandomWaypoints waypoint = GetComponent<PatrolRandomWaypoints>();
            if (waypoint != null)
            {
                waypoint.enabled = false;
            }
        }
    }

}