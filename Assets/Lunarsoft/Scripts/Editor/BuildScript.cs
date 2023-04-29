using UnityEditor;
using System.IO;

public class BuildScript
{
    public static void BuildProject()
    {
        // Define the name of the build
        string buildName = "MyUnityBuild";

        // Define the output directory for the build
        string outputPath = Path.Combine("Builds", buildName);

        // Define the build target (e.g., StandaloneOSX for macOS)
        BuildTarget buildTarget = BuildTarget.StandaloneOSX;

        // Define the build options (e.g., Development build)
        BuildOptions buildOptions = BuildOptions.Development;

        // Get the scenes to build from the current build settings
        string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

        // Run the Unity build command
        BuildPipeline.BuildPlayer(scenes, outputPath, buildTarget, buildOptions);
    }
}
