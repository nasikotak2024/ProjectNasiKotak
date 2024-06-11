using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll(); // Opsional: Reset game progress
        SceneManager.LoadScene("Halaman Kampus"); // Ganti dengan nama scene pertama Anda
    }

    public void Continue()
    {
        if (gameManager != null)
        {
            gameManager.LoadGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
