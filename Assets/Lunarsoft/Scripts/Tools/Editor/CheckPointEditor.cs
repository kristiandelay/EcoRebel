using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Lunarsoft
{
    [CustomEditor(typeof(CheckPoint))]
    public class CheckPointEditor : Editor
    {
        SerializedProperty activeObject;
        SerializedProperty checkpointActive;
        SerializedProperty checkpointCollider;
        SerializedProperty animator;
        SerializedProperty spawnLocation;
        SerializedProperty playerPrefab;
        SerializedProperty herosJourneyStep;
        SerializedProperty sceneToLoad;
        SerializedProperty virtualCamera;

        

        void OnEnable()
        {
            activeObject = serializedObject.FindProperty("activeObject");
            checkpointActive = serializedObject.FindProperty("checkpointActive");
            checkpointCollider = serializedObject.FindProperty("checkpointCollider");
            animator = serializedObject.FindProperty("animator");
            spawnLocation = serializedObject.FindProperty("spawnLocation");
            playerPrefab = serializedObject.FindProperty("playerPrefab");
            herosJourneyStep = serializedObject.FindProperty("herosJourneyStep");
            sceneToLoad = serializedObject.FindProperty("sceneToLoad");
            virtualCamera = serializedObject.FindProperty("virtualCamera");
            
        }

        public override void OnInspectorGUI()
        {
            // Draw the other fields
            EditorGUILayout.PropertyField(activeObject);
            EditorGUILayout.PropertyField(checkpointActive);
            EditorGUILayout.PropertyField(checkpointCollider);
            EditorGUILayout.PropertyField(animator);
            EditorGUILayout.PropertyField(spawnLocation);
            EditorGUILayout.PropertyField(playerPrefab);
            EditorGUILayout.PropertyField(herosJourneyStep);
            EditorGUILayout.PropertyField(virtualCamera);

            // Get the list of scenes in the build settings
            List<string> sceneNames = new List<string>();
            for (int i = 0; i < EditorSceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                sceneNames.Add(sceneName);
            }

            // Create a dropdown list to select the scene
            int selectedIndex = sceneNames.IndexOf(sceneToLoad.stringValue);
            selectedIndex = EditorGUILayout.Popup("Scene To Load", selectedIndex, sceneNames.ToArray());
            if (selectedIndex >= 0 && selectedIndex < sceneNames.Count)
            {
                sceneToLoad.stringValue = sceneNames[selectedIndex];
            }

            // Apply the changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
}