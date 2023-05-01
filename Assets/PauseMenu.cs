using UnityEngine;
using UnityEngine.UI;

namespace Lunarsoft
{
    public class PauseMenu : MonoBehaviour
    {
        // Reference to the pause menu prefab
        public GameObject pauseMenuPrefab;

        public GameObject defeatMenuPrefab;

        // Reference to the settings GameObject
        public GameObject settingsMenu;

        // References to the buttons
        public Button settingsButton;
        public Button continueButton;
        public Button giveUpButton;

        // Boolean to track whether the game is paused
        private bool isPaused;
        private bool isShowingDefeat;

        // Boolean to track whether the settings menu is open
        private bool isSettingsOpen;

        // Singleton instance
        private static PauseMenu instance;

        private void Awake()
        {
            // Check if an instance of PauseMenu already exists
            if (instance == null)
            {
                // If not, set this as the instance
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // If an instance already exists, destroy this duplicate
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            // Ensure that the PauseMenu GameObject is not destroyed when loading a new scene
            DontDestroyOnLoad(gameObject);

            // Add click event listeners to the buttons
            settingsButton.onClick.AddListener(OpenSettings);
            continueButton.onClick.AddListener(ClosePauseMenu);
            giveUpButton.onClick.AddListener(GiveUp);

            // Initially hide the settings menu
            settingsMenu.SetActive(false);
            defeatMenuPrefab.SetActive(false);
            isShowingDefeat = false;
        }

        private void Update()
        {
            // Check if the Escape key is pressed
            if (Input.GetKeyDown(KeyCode.Escape) && isShowingDefeat != true)
            {
                // Toggle the pause state
                TogglePause();
            }
        }

        private void TogglePause()
        {
            // If the game is not paused, pause it and show the pause menu
            if (!isPaused)
            {
                // Show the pause menu
                pauseMenuPrefab.SetActive(true);

                // Pause the game by setting time scale to 0
                Time.timeScale = 0f;

                // Set the pause state to true
                isPaused = true;
            }
            else
            {
                // Hide the pause menu
                pauseMenuPrefab.SetActive(false);

                // Resume the game by setting time scale to 1
                Time.timeScale = 1f;

                // Set the pause state to false
                isPaused = false;

                CloseSettings();
            }
        }

        private void OpenSettings()
        {
            // Show the settings menu
            settingsMenu.SetActive(true);

            // Set the settings open state to true
            isSettingsOpen = true;
        }

        public void CloseSettings()
        {
            // Hide the settings menu
            settingsMenu.SetActive(false);

            // Set the settings open state to false
            isSettingsOpen = false;
        }

        private void ClosePauseMenu()
        {
            // Hide the pause menu and resume the game
            TogglePause();
        }

        public void ShowDefeatMenu()
        {
            defeatMenuPrefab.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            isShowingDefeat = true;
        }

        public void HideDefeatMenu()
        {
            defeatMenuPrefab.SetActive(false);
        }

        public void GiveUp()
        {
            // Close the application
            Application.Quit();
        }
    }
}
