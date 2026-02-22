using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [Header("Menus")]
    [SerializeField] GameObject _mainMenu;
    [SerializeField] SettingsMenu _settingsMenu;
    [SerializeField] GameObject _startMenu;

    void Start()
    {
        CursorLock.Ref.UnlockCursor();
    }

    // Starts the game into the main scene
    public void StartGame()
    {
        // This uses the second scene in the build order which should be the main scene
        SceneManager.LoadScene("Scenes/Arena_Level");
    }

    public void ExtendStartMenu()
    {
        _startMenu.SetActive(true);
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
        _startMenu.SetActive(false);
    }

    public void StartHardGame()
    {
        DifficultyAuthority.Ref.UpdateDifficulty(eDifficulty.Hard);
        StartGame();
    }

    public void StartExtremeGame()
    {
        DifficultyAuthority.Ref.UpdateDifficulty(eDifficulty.Extreme);
        StartGame();
    }

    public void StartImpossibleGame()
    {
        DifficultyAuthority.Ref.UpdateDifficulty(eDifficulty.Impossible);
        StartGame();
    }

}