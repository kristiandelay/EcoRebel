using UnityEngine;
using UnityEditor;
using Lunarsoft;

public class ApiSettingsEditorWindow : EditorWindow
{
    private string bearerToken;

    [MenuItem("Lunarsoft/Api Settings")]
    public static void ShowWindow()
    {
        var window = GetWindow<ApiSettingsEditorWindow>("Api Settings");
        window.LoadToken();
    }

    private void LoadToken()
    {
        bearerToken = PlayerPrefs.GetString("bearerToken", string.Empty);
    }

    private void OnGUI()
    {
        GUILayout.Label("API Settings", EditorStyles.boldLabel);

        bearerToken = EditorGUILayout.TextField("Bearer Token", bearerToken);

        if (GUILayout.Button("Save"))
        {
            PlayerPrefs.SetString("bearerToken", bearerToken);
        }
    }
}
