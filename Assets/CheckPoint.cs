using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class CheckPoint : MonoBehaviour
    {

        public GameObject activeObject;
        public bool checkpointActive;

        public BoxCollider2D checkpointCollider;
        public Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            checkpointCollider = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Activate()
        {
            if (checkpointActive == false)
            {
                animator.SetTrigger("Activated");
                checkpointActive = true;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Activate();
            }
        }
    }
}
