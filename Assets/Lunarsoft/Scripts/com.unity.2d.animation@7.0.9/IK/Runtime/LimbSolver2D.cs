using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.U2D.IK
{
    /// <summary>
    /// Component for 2D Limb IK.
    /// </summary>
    [MovedFrom("UnityEngine.Experimental.U2D.IK")]
    [Solver2DMenuAttribute("Limb")]
    public class LimbSolver2D : Solver2D
    {
        [SerializeField]
        IKChain2D m_Chain = new IKChain2D();

        [SerializeField]
        bool m_Flip;

        Vector3[] m_Positions = new Vector3[3];
        float[] m_Lengths = new float[2];
        float[] m_Angles = new float[2];

        /// <summary>
        /// Get Set for flip property.
        /// </summary>
        public bool flip
        {
            get => m_Flip;
            set => m_Flip = value;
        }

        /// <summary>
        /// Override base class DoInitialize.
        /// </summary>
        protected override void DoInitialize()
        {
            m_Chain.transformCount = m_Chain.effector == null || IKUtility.GetAncestorCount(m_Chain.effector) < 2 ? 0 : 3;
            base.DoInitialize();
        }

        /// <summary>
        /// Override base class GetChainCount.
        /// </summary>
        /// <returns>Always returns 1.</returns>
        protected override int GetChainCount() => 1;

        /// <summary>
        /// Override base class GetChain.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Returns IKChain2D for the Solver.</returns>
        public override IKChain2D GetChain(int index) => m_Chain;

        /// <summary>
        /// Override base class DoPrepare.
        /// </summary>
        protected override void DoPrepare()
        {
            var lengths = m_Chain.lengths;
            m_Positions[0] = m_Chain.transforms[0].position;
            m_Positions[1] = m_Chain.transforms[1].position;
            m_Positions[2] = m_Chain.transforms[2].position;
            m_Lengths[0] = lengths[0];
            m_Lengths[1] = lengths[1];
        }

        /// <summary>
        /// Override base class DoUpdateIK.
        /// </summary>
        /// <param name="effectorPositions">List of effector positions.</param>
        protected override void DoUpdateIK(List<Vector3> effectorPositions)
        {
            var effectorPosition = effectorPositions[0];
            var effectorLocalPosition2D = m_Chain.transforms[0].InverseTransformPoint(effectorPosition);
            effectorPosition = m_Chain.transforms[0].TransformPoint(effectorLocalPosition2D);

            if (effectorLocalPosition2D.sqrMagnitude > 0f && Limb.Solve(effectorPosition, m_Lengths, m_Positions, ref m_Angles))
            {
                var flipSign = flip ? -1f : 1f;
                m_Chain.transforms[0].localRotation *= Quaternion.FromToRotation(Vector3.right, effectorLocalPosition2D) * Quaternion.FromToRotation(m_Chain.transforms[1].localPosition, Vector3.right);
                m_Chain.transforms[0].localRotation *= Quaternion.AngleAxis(flipSign * m_Angles[0], Vector3.forward);
                m_Chain.transforms[1].localRotation *= Quaternion.FromToRotation(Vector3.right, m_Chain.transforms[1].InverseTransformPoint(effectorPosition)) * Quaternion.FromToRotation(m_Chain.transforms[2].localPosition, Vector3.right);
            }
        }

        public virtual void setEffector(Transform effectorTarget)
        {
            if (m_Chain != null)
            {
                m_Chain.effector = effectorTarget;
            }
        }

        public virtual void createTarget()
        {
            IKChain2D chain = GetChain(GetChainCount());
            if (chain != null && chain.target == null && chain.effector != null)
            {
                chain.target = new GameObject(name + "_Target").transform;
                chain.target.SetParent(transform);
                chain.target.position = chain.effector.position;
                chain.target.rotation = chain.effector.rotation;

            }
        }
    }
}