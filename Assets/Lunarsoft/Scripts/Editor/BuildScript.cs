using UnityEditor;
using System.IO;

public class BuildScript
{
    public static void BuildMacProject()
    {
        // Define the name of the build
        string buildName = "EcoRebel";

        // Define the output directory for the build
        string outputPath = Path.Combine("Builds/MacOS", buildName);

        // Define the build target (e.g., StandaloneOSX for macOS)
        BuildTarget buildTarget = BuildTarget.StandaloneOSX;

        // Define the build options (e.g., Development build)
        BuildOptions buildOptions = BuildOptions.Development;

        // Get the scenes to build from the current build settings
        string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

        // Run the Unity build command
        BuildPipeline.BuildPlayer(scenes, outputPath, buildTarget, buildOptions);
    }

    public static void BuildWindowsProject()
    {
        // Define the name of the build
        string buildName = "EcoRebel.exe";

        // Define the output directory for the build
        string outputPath = Path.Combine("Builds/Windows64", buildName);

        // Define the build target (e.g., StandaloneOSX for StandaloneWindows64)
        BuildTarget buildTarget = BuildTarget.StandaloneWindows64;

        // Define the build options (e.g., Development build)
        BuildOptions buildOptions = BuildOptions.Development;

        // Get the scenes to build from the current build settings
        string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

        // Run the Unity build command
        BuildPipeline.BuildPlayer(scenes, outputPath, buildTarget, buildOptions);
    }
}
