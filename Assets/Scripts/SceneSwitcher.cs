using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // This method is called when the script is enabled
    private void OnEnable()
    {
        // Load the "MainMenu" scene
        SceneManager.LoadScene("MainMenu");
    }
}
 