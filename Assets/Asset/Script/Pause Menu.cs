using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button saveGameButton;
    public Button mainMenuButton;
    public Button quitGameButton;

    private GameManager gameManager;
    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        gameManager = GameManager.instance;

        saveGameButton.onClick.AddListener(SaveGame);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    void SaveGame()
    {
        if (gameManager != null)
        {
            gameManager.SaveGame();
        }
    }

    void MainMenu()
    {
        Time.timeScale = 1f; // Resume game time
        SceneManager.LoadScene("Main_Menu"); // Ganti dengan nama scene Main Menu Anda
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
