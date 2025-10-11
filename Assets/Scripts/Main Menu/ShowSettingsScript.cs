using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowSettingsScript : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static ShowSettingsScript instance;

    // The name of your settings scene
    private string settingsSceneName = "SettingsMenu";

    void Awake()
    {
        // If an instance of this script already exists, destroy this new one.
        if (instance != null)
        {
            Destroy(gameObject);
            return; // Stop running code in this new, duplicate instance.
        }
        
        // If this is the first instance, make it the official one and don't destroy it.
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Check if the 'M' key is pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Check if the settings menu is NOT already loaded
            if (!IsSceneLoaded(settingsSceneName))
            {
                // Load the settings menu ON TOP of the current scene
                SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
            }
        }
    }

    // Helper function to check if a scene is loaded
    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}