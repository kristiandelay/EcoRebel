using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using Lunarsoft;

public class AgentAnimator : MonoBehaviour
{

    private PolyNavAgent _agent;
    private PolyNavAgent agent
    {
        get { return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>(); }
    }



    private AIController _controller;
    private AIController controller
    {
        get { return _controller != null ? _controller : _controller = GetComponent<AIController>(); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controller?.animator != null)
        {
            controller.animator.SetFloat("Speed", agent.currentSpeed);
        }
    }
}
