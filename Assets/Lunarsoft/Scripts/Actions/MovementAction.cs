using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class MovementAction : BaseAction
    {

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            controller.horizontalInput = Input.GetAxis("Horizontal");
            controller.verticalInput = Input.GetAxis("Vertical");
            //bool isAttacking = controller.GetComponent<LightAttackAction>().isAttacking;

            //if(isAttacking)
            //{
            //    return;
            //}

            controller.Move(new Vector2(controller.horizontalInput, controller.verticalInput));
            FaceDirection(controller.horizontalInput);
        }

        private void FaceDirection(float horizontal)
        {
            if (horizontal > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(controller.originalScale.x), controller.originalScale.y, controller.originalScale.z);
                controller.facingRight = true;
            }
            else if (horizontal < 0)
            {
                controller.facingRight = false;
                transform.localScale = new Vector3(-Mathf.Abs(controller.originalScale.x), controller.originalScale.y, controller.originalScale.z);
            }
        }
    }
}