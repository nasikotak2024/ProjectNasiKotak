using UnityEngine;
using UnityEngine.UI;

public class SaveGameButton : MonoBehaviour
{
    private Button button;
    private GameManager gameManager;

    void Start()
    {
        button = GetComponent<Button>();
        gameManager = FindObjectOfType<GameManager>();

        if (button != null && gameManager != null)
        {
            button.onClick.AddListener(SaveGame);
        }
        else
        {
            Debug.LogError("Button or GameManager not found.");
        }
    }

    void SaveGame()
    {
        gameManager.SaveGame();
    }
}
