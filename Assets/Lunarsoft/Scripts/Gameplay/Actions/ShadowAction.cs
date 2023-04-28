using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class ShadowAction : BaseAction
    {
        [SerializeField] public Transform shadowTransform;
        [SerializeField] public Vector3 shadowOffset;
        [SerializeField] public Vector3 minShadowScale = new Vector3(3.2187f, 3.2187f, 4.2187f);
        [SerializeField] public Vector3 maxShadowScale = new Vector3(4.2187f, 4.2187f, 4.2187f);
        public float yOffset;

        [SerializeField] public GameObject shadowPrefab;

        private JumpAction jumpAction;


        override protected void Start()
        {
            base.Start();

            // Instantiate the shadow object without setting it as a child of the player
            GameObject shadowObject = Instantiate(shadowPrefab, transform.position + shadowOffset, Quaternion.identity);
            shadowTransform = shadowObject.transform;
            yOffset = Mathf.Abs(transform.position.y - shadowTransform.position.y);

            jumpAction = controller.GetComponent<JumpAction>();

        }

        override protected void Update()
        {
            Vector3 shadowPosition = new Vector3(transform.position.x + shadowOffset.x, shadowTransform.position.y, transform.position.z + shadowOffset.z);

            if (jumpAction != null && jumpAction.isJumping)
            {
                // Keep the shadow's y position constant when jumping
                shadowTransform.position = shadowPosition;
            }
            else
            {
                // Move the shadow with the player when not jumping, and apply the shadow offset
                shadowTransform.position = new Vector3(shadowPosition.x, transform.position.y - yOffset + shadowOffset.y, shadowPosition.z);
            }
        }

    }

}