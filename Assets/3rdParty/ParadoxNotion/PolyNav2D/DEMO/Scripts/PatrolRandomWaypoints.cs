using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PolyNav;


//example. moving between some points at random
[RequireComponent(typeof(PolyNavAgent))]
public class PatrolRandomWaypoints : MonoBehaviour
{
    public List<Transform> WPoints = new List<Transform>();
    public float delayBetweenPoints = 0f;

    private PolyNavAgent _agent;
    private PolyNavAgent agent {
        get { return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>(); }
    }

    void OnEnable() {

        agent.OnDestinationReached += MoveRandom;
        agent.OnDestinationInvalid += MoveRandom;

        if (WPoints.Count > 0)
        {
            MoveRandom();
        }

    }

    void OnDisable() {
        agent.OnDestinationReached -= MoveRandom;
        agent.OnDestinationInvalid -= MoveRandom;
    }

    void Start() {
        if ( WPoints.Count > 0 ) {
            MoveRandom();
        }
    }

    void MoveRandom() {
        StartCoroutine(WaitAndMove());
    }

    IEnumerator WaitAndMove() {
        float randomDelay = Random.Range(0f, delayBetweenPoints);
        yield return new WaitForSeconds(randomDelay);
        agent.SetDestination(WPoints[Random.Range(0, WPoints.Count)].position);
    }

    void OnDrawGizmosSelected() {
        for ( int i = 0; i < WPoints.Count; i++ ) {
            Gizmos.DrawSphere(WPoints[i].position, 0.05f);
        }
    }
}
