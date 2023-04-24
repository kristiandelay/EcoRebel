using PolyNav;
using UnityEngine;

namespace Lunarsoft
{
    public abstract class AIStateBase : MonoBehaviour
    {
        public bool enableDebugLogs = false;
        public AIController controller;
        public FindClosestEnemy findClosestEnemy;
        protected PolyNavAgent agent;

        // Non-virtual methods that call the abstract methods
        public void EnterState()
        {
            if (enableDebugLogs) Debug.Log("EnterState");
            controller = GetComponent<AIController>();
            findClosestEnemy = GetComponent<FindClosestEnemy>();
            agent = GetComponent<PolyNavAgent>();
            OnEnterState();
        }

        public void UpdateState()
        {
            if (enableDebugLogs) Debug.Log("UpdateState");
            OnUpdateState();
        }

        public void ExitState()
        {
            if (enableDebugLogs) Debug.Log("ExitState");
            OnExitState();
        }

        // Abstract methods for derived classes to implement specific behavior
        protected abstract void OnEnterState();
        protected abstract void OnUpdateState();
        protected abstract void OnExitState();
    }
}
