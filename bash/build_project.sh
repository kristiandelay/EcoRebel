#!/bin/bash

# Define the Discord webhook URL (replace with your actual webhook URL)
DISCORD_WEBHOOK_URL="https://discord.com/api/webhooks/1101688868979355719/4Ur_6gmju_YAvaoQgj0c4op9w9vNNED1_bsek5LAvsqvwGTq96lMC8iTjX4aSW7OPRcx"

# Define the path to the Unity installation on your Mac
UNITY_PATH="/Applications/Unity/Hub/Editor/2021.3.21f1/Unity.app/Contents/MacOS/Unity"

# Define the path to the Unity project you want to build
PROJECT_PATH="/Users/kristian/lunarsoft/Unity/GoedWareGameJam8"

# Change to the Unity project directory
cd "$PROJECT_PATH"

# Get the URL of the remote repository (usually "origin")
REMOTE_URL=$(git remote get-url origin)

# Extract the GitHub owner and repository name from the remote URL, and remove the ".git" suffix
GITHUB_OWNER_AND_REPO=$(echo "$REMOTE_URL" | sed -n 's/.*github.com[:/]\([^/]*\/[^/]*\)\.git/\1/p')

# Get the last Git commit hash, message, and author name
LAST_COMMIT_HASH=$(git log -1 --pretty=format:"%h")
LAST_COMMIT_MESSAGE=$(git log -1 --pretty=format:"%s")
LAST_COMMIT_AUTHOR=$(git log -1 --pretty=format:"%an")

# Construct the URL to the GitHub commit
GITHUB_COMMIT_URL="https://github.com/$GITHUB_OWNER_AND_REPO/commit/$LAST_COMMIT_HASH"

# Check if an argument was provided
if [ -z "$1" ]; then
  # Prompt the user to choose the build target
  echo "Please choose the build target:"
  echo "1. Windows"
  echo "2. Mac"
  read -p "Enter your choice (1 or 2): " choice

  # Set the build method based on the user's choice
  if [ "$choice" == "1" ]; then
    BUILD_METHOD="BuildScript.BuildWindowsProject"
    BUILD_TARGET_NAME="Windows"
    UNITY_PATH="C:\Program Files\Unity\Hub\Editor\2021.3.21f1\Editor\Unity.exe"
    PROJECT_PATH="L:\Lunarsoft\Unity\EcoRebel"
  elif [ "$choice" == "2" ]; then
    BUILD_METHOD="BuildScript.BuildMacProject"
    BUILD_TARGET_NAME="Mac"
  else
    echo "Invalid choice. Exiting."
    exit 1
  fi
else
  # Set the build method based on the command-line argument
  if [ "$1" == "windows" ]; then
    BUILD_METHOD="BuildScript.BuildWindowsProject"
    BUILD_TARGET_NAME="Windows"
    UNITY_PATH="C:\Program Files\Unity\Hub\Editor\2021.3.21f1\Editor\Unity.exe"
    PROJECT_PATH="L:\Lunarsoft\Unity\EcoRebel"
  elif [ "$1" == "mac" ]; then
    BUILD_METHOD="BuildScript.BuildMacProject"
    BUILD_TARGET_NAME="Mac"
  else
    echo "Invalid argument. Please specify 'windows' or 'mac'."
    exit 1
  fi
fi

# Send the last Git commit information to Discord
curl -X POST -H "Content-Type: application/json" \
  -d "{\"content\":\"Starting Unity build for $BUILD_TARGET_NAME. Last Git commit: [$LAST_COMMIT_HASH]($GITHUB_COMMIT_URL) - $LAST_COMMIT_MESSAGE (Author: $LAST_COMMIT_AUTHOR)\"}" \
  "$DISCORD_WEBHOOK_URL"

# Get the current timestamp (start time)
START_TIME=$(date +%s)

# Run the Unity build command with the chosen build method
"$UNITY_PATH" -batchmode -nographics -silent-crashes -quit \
  -projectPath "$PROJECT_PATH" \
  -executeMethod "$BUILD_METHOD"

# Get the exit code of the Unity build command
UNITY_EXIT_CODE=$?

# Get the current timestamp (end time)
END_TIME=$(date +%s)

# Calculate the time difference (build duration)
BUILD_DURATION=$((END_TIME - START_TIME))


# Check the exit code to see if the build was successful
if [ $UNITY_EXIT_CODE -eq 0 ]; then
  # Send a success message to Discord, including the last Git commit information and build duration
  curl -X POST -H "Content-Type: application/json" \
    -d "{\"content\":\"Unity build for $BUILD_TARGET_NAME completed successfully in $BUILD_DURATION seconds. Last Git commit: [$LAST_COMMIT_HASH]($GITHUB_COMMIT_URL) - $LAST_COMMIT_MESSAGE (Author: $LAST_COMMIT_AUTHOR)\"}" \
    "$DISCORD_WEBHOOK_URL"
else
  # Send a failure message to Discord, including the last Git commit information and build duration
  curl -X POST -H "Content-Type: application/json" \
    -d "{\"content\":\"Unity build for $BUILD_TARGET_NAME failed after $BUILD_DURATION seconds. Last Git commit: [$LAST_COMMIT_HASH]($GITHUB_COMMIT_URL) - $LAST_COMMIT_MESSAGE (Author: $LAST_COMMIT_AUTHOR)\"}" \
    "$DISCORD_WEBHOOK_URL"
fi