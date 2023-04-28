using System.Collections;
using UnityEngine;

namespace Lunarsoft
{
    public class DashAction : BaseAction
    {
        [SerializeField] private float dashSpeed = 10f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float doubleTapInterval = 0.3f;

        private bool isDashing = false;
        private float lastTapTimeHorizontal = 0f;
        private float lastTapTimeVertical = 0f;
        private float lastHorizontalInput = 0f;
        private float lastVerticalInput = 0f;

        protected override void Update()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (!isDashing)
            {
                CheckForDashInput(horizontalInput, verticalInput);
            }
        }

        private void CheckForDashInput(float horizontalInput, float verticalInput)
        {
            if (Mathf.Abs(horizontalInput) > 0 && horizontalInput != lastHorizontalInput)
            {
                if (Time.time - lastTapTimeHorizontal < doubleTapInterval)
                {
                    StartCoroutine(Dash(new Vector2(horizontalInput, 0)));
                }
                else
                {
                    lastTapTimeHorizontal = Time.time;
                }
                lastHorizontalInput = horizontalInput;
            }
            else if (Mathf.Abs(verticalInput) > 0 && verticalInput != lastVerticalInput)
            {
                if (Time.time - lastTapTimeVertical < doubleTapInterval)
                {
                    StartCoroutine(Dash(new Vector2(0, verticalInput)));
                }
                else
                {
                    lastTapTimeVertical = Time.time;
                }
                lastVerticalInput = verticalInput;
            }

            controller.horizontalInput = horizontalInput;
            controller.verticalInput = verticalInput;
        }

        private IEnumerator Dash(Vector2 direction)
        {
            isDashing = true;
            float startTime = Time.time;

            while (Time.time < startTime + dashDuration)
            {
                controller.Move(direction.normalized * dashSpeed);
                yield return null;
            }

            isDashing = false;
        }
    }
}
