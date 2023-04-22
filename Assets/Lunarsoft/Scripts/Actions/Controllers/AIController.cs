using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunarsoft
{
    public class AIController : BaseController
    {
        [SerializeField] private Vector2 direction = Vector2.right;

        public override void Die()
        {
        }

        public override void TakeDamage(float damage, string animationTrigger = "Hit")
        {
            animator.SetTrigger(animationTrigger);
        }

        protected override void Start()
        {
            base.Start();
            // Add any AI-specific setup code here
        }

        protected override void Update()
        {
        }
    }
}