using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class BaseAction : MonoBehaviour
    {
        public BaseController controller;

        // Start is called before the first frame update
        virtual protected void Start()
        {
            if (controller == null)
            {
                controller = GetComponent<BaseController>();
            }
        }

        // Update is called once per frame
        virtual protected void Update()
        {
        }

        virtual protected void FixedUpdate()
        {

        }

        virtual protected void Flip()
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(controller.horizontalInput), transform.localScale.y, transform.localScale.z);
            controller.facingRight = !controller.facingRight;
        }
    }

}

