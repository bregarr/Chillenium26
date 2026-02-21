using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [Header("Menus")]
    [SerializeField] GameObject _mainMenu;
    [SerializeField] SettingsMenu _settingsMenu;

    // Starts the game into the main scene
    public void StartGame()
    {
        // This uses the second scene in the build order which should be the main scene
        SceneManager.LoadScene("Scenes/Testing/CoreTest");
    }

    public void OpenSettingsMenu()
    {
        DisableAllMenus();
        _settingsMenu.EnableMenu();
    }

    public void OpenMainMenu()
    {
        DisableAllMenus();
        _mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void DisableAllMenus()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.DisableMenu();
    }

}