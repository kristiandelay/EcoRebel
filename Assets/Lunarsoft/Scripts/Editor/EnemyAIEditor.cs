using UnityEngine;
using UnityEditor;

namespace Lunarsoft
{
    [CustomEditor(typeof(EnemyAI))]
    public class EnemyAIEditor : Editor
    {
        private void OnSceneGUI()
        {
            EnemyAI enemyAI = (EnemyAI)target;

            if (enemyAI.patrolPath != null && enemyAI.patrolPath.points.Length > 1)
            {
                for (int i = 0; i < enemyAI.patrolPath.points.Length; i++)
                {
                    Vector3 worldPoint = enemyAI.patrolPath.transform.TransformPoint(enemyAI.patrolPath.points[i]);
                    int nextIndex = (i + 1) % enemyAI.patrolPath.points.Length;
                    Vector3 nextWorldPoint = enemyAI.patrolPath.transform.TransformPoint(enemyAI.patrolPath.points[nextIndex]);

                    Handles.color = Color.green;
                    Handles.DrawLine(worldPoint, nextWorldPoint);
                    Handles.SphereHandleCap(0, worldPoint, Quaternion.identity, 0.5f, EventType.Repaint);
                }
            }
        }
    }
}