#!/bin/bash

# Define the path to the Unity installation on your Mac
UNITY_PATH="/Applications/Unity/Hub/Editor/2021.3.21f1/Unity.app/Contents/MacOS/Unity"

# Define the path to the Unity project you want to build
PROJECT_PATH="/Users/kristian/lunarsoft/Unity/GoedWareGameJam8"

# Run the Unity build command
"$UNITY_PATH" -batchmode -nographics -silent-crashes -quit \
  -projectPath "$PROJECT_PATH" \
  -executeMethod BuildScript.BuildProject

# Check the exit code to see if the build was successful
if [ $? -eq 0 ]; then
  echo "Unity build completed successfully."
else
  echo "Unity build failed."
fi