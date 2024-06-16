using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject continueButton;

    private void Start()
    {

        gameManager = GameManager.instance;
        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void NewGame()
    {        
        Time.timeScale = 1f;
        PlayerPrefs.DeleteAll(); // Opsional: Reset game progress
        SceneManager.LoadScene("Cutscene"); // Ganti dengan nama scene pertama Anda
    }

    public void Continue()
    {
        if (gameManager != null)
        {
            Time.timeScale = 1f;
            gameManager.LoadGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
