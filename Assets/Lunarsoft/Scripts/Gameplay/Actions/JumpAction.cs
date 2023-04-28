using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class JumpAction : BaseAction
    {
        [SerializeField] public float jumpHeight = 2f;
        [SerializeField] public float jumpDuration = 0.5f;

        [SerializeField] public bool isJumping = false;

        private ShadowAction shadowAction;


        override protected void Start()
        {
            base.Start();
            shadowAction = controller.GetComponent<ShadowAction>();
        }

        override protected void Update()
        {
            if (!isJumping && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(PseudoJump());
            }
        }

        private IEnumerator PseudoJump()
        {
            isJumping = true;

            float startTime = Time.time;
            float halfDuration = jumpDuration / 2f;
            float originalY = transform.position.y;
            float targetY = originalY + jumpHeight;
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * 0.8f, originalScale.z);

            // Jump up
            while (Time.time < startTime + halfDuration)
            {
                float t = (Time.time - startTime) / halfDuration;
                float newY = Mathf.Lerp(originalY, targetY, t);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
                transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
                if(shadowAction != null)
                {
                    shadowAction.shadowTransform.localScale = Vector3.Lerp(shadowAction.maxShadowScale, shadowAction.minShadowScale, t);
                }
                yield return null;
            }

            // Jump down
            startTime = Time.time;
            while (Time.time < startTime + halfDuration)
            {
                float t = (Time.time - startTime) / halfDuration;
                float newY = Mathf.Lerp(targetY, originalY, t);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
                transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
                if (shadowAction != null)
                {
                    shadowAction.shadowTransform.localScale = Vector3.Lerp(shadowAction.minShadowScale, shadowAction.maxShadowScale, t);
                }
                yield return null;
            }

            transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
            transform.localScale = originalScale;
            if (shadowAction != null)
            {
                shadowAction.shadowTransform.localScale = shadowAction.maxShadowScale;
            }
            isJumping = false;
        }

    }
}