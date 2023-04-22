using UnityEngine;
using UnityEditor;
using Lunarsoft;

[InitializeOnLoad]
public class TokenChecker
{
    static TokenChecker()
    {
        EditorApplication.delayCall += CheckToken;
    }

    private static void CheckToken()
    {
        string storedToken = PlayerPrefs.GetString("bearerToken", string.Empty);

        if (string.IsNullOrEmpty(storedToken))
        {
            ApiSettingsEditorWindow.ShowWindow();
        }
    }
}
