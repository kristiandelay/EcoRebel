using UnityEngine;

public class EasterEggs : MonoBehaviour
{
    // Define the Konami Code sequence
    private KeyCode[] konamiCode = {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A
    };

    // Define the Mortal Kombat blood code sequence for the SNES
    private KeyCode[] mkBloodCode = {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.A,
        KeyCode.B,
        KeyCode.Return
    };


    // Keep track of the current position in the Konami Code sequence
    private int currentPosition = 0;

    // Update is called once per frame
    void Update()
    {
        // Check if the user has pressed a key in the Konami Code sequence
        if (Input.GetKeyDown(konamiCode[currentPosition]))
        {
            // Advance to the next position in the Konami Code sequence
            currentPosition++;

            // Check if the entire Konami Code sequence has been entered correctly
            if (currentPosition == konamiCode.Length)
            {
                // Reset the current position to 0 so the Konami Code can be entered again
                currentPosition = 0;

                // Perform the desired action when the Konami Code is triggered
                Debug.Log("Konami Code triggered!");
            }
        }
        else if (Input.anyKeyDown)
        {
            // Reset the current position to 0 if the user presses the wrong key
            currentPosition = 0;
        }
    }
}
