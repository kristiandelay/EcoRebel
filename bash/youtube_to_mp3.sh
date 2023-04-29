#!/bin/bash

# Function to install youtube-dl
install_youtube_dl() {
  if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    # Check if apt is available (Debian/Ubuntu-based systems)
    if command -v apt &> /dev/null; then
      sudo apt update
      sudo apt install youtube-dl
    else
      # Fallback to pip
      pip install youtube-dl
    fi
  elif [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS
    brew install youtube-dl
  else
    # Fallback to pip
    pip install youtube-dl
  fi
}

# Check if youtube-dl is installed, if not, install it
if ! command -v youtube-dl &> /dev/null; then
  echo "youtube-dl is not installed. Installing it now..."
  install_youtube_dl
fi

# Check if ffmpeg is installed
if ! command -v ffmpeg &> /dev/null; then
  echo "ffmpeg is not installed. Please install it and try again."
  exit 1
fi

# Loop to continuously wait for user input and process URLs
while true; do
  # Prompt the user for a URL
  echo "Please enter a YouTube URL (or type 'exit' to quit):"
  read url

  # Exit the loop if the user types 'exit'
  if [[ "$url" == "exit" ]]; then
    break
  fi

  # Download the video and convert it to MP3
  youtube-dl -x --audio-format mp3 "$url"

  echo "The MP3 file has been saved locally."
done

echo "Exiting the script."
