using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Lunarsoft
{
    [CustomEditor(typeof(Door2D))]
    public class Door2DEditor : Editor
    {
        private Door2D door;

        private void OnEnable()
        {
            door = (Door2D)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // Scene drop-down
            string[] sceneNames = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
            int currentSceneIndex = door.sceneIndex;

            int sceneSelectedIndex = EditorGUILayout.Popup("Scene to Load", currentSceneIndex, sceneNames);
            if (sceneSelectedIndex != currentSceneIndex)
            {
                door.sceneIndex = sceneSelectedIndex;
                EditorUtility.SetDirty(door);
            }

            // Player tag drop-down
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
            string currentPlayerTag = door.playerTag;

            int tagSelectedIndex = EditorGUILayout.Popup("Player Tag", System.Array.IndexOf(tags, currentPlayerTag), tags);
            string selectedTag = tags[tagSelectedIndex];
            if (selectedTag != currentPlayerTag)
            {
                door.playerTag = selectedTag;
                EditorUtility.SetDirty(door);
            }

            // Spawn point tag drop-down
            string currentSpawnPointTag = door.spawnPointTag;

            int spawnPointTagSelectedIndex = EditorGUILayout.Popup("Spawn Point Tag", System.Array.IndexOf(tags, currentSpawnPointTag), tags);
            string selectedSpawnPointTag = tags[spawnPointTagSelectedIndex];
            if (selectedSpawnPointTag != currentSpawnPointTag)
            {
                door.spawnPointTag = selectedSpawnPointTag;
                EditorUtility.SetDirty(door);
            }
        }
    }
}
