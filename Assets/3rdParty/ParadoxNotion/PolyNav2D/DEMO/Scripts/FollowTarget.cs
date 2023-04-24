using UnityEngine;
using PolyNav;

[RequireComponent(typeof(PolyNavAgent))]
public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float distanceFromTarget;

    private PolyNavAgent _agent;
    private PolyNavAgent agent {
        get { return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>(); }
    }

    public void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            distanceFromTarget = CalculateDistanceFromTarget();
        }
    }

    public float CalculateDistanceFromTarget()
    {
        return Vector3.Distance(transform.position, target.position);
    }
}
