using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] string playScene = "PlayerSandboxScene";
    [SerializeField] string mainMenuScene = "StartScene";
   
    [Tooltip("Drag in an options menu panel, if one exists")]
    [SerializeField] GameObject optionsMenuPanel;

    [Tooltip("Drag in an pause menu panel, if one exists")]
    [SerializeField] GameObject pauseMenuPanel;

    [SerializeField] bool IsPauseMenuAvailable = false;
    [HideInInspector] public static bool IsGamePaused = false;

    void Update()
    {
        PauseMenu();
    }

    public void PauseMenu()
    {
        if (IsPauseMenuAvailable)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsGamePaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void OptionsMenuClose()
    {
        optionsMenuPanel.SetActive(false);
    }

    public void OptionsMenuOpen()
    {
        optionsMenuPanel.SetActive(true);
    }

    public void Pause()
    {
        Cursor.visible = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void Resume()
    {
        Cursor.visible = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void ReturnToMainMenu()
    {
        Resume();
        Cursor.visible = true;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void StartGame()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(playScene);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}
