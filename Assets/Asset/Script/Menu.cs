using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOver : MonoBehaviour
{
    public Button mainMenuGameButton;


    void Start()
    {

        mainMenuGameButton.onClick.AddListener(MainMenu);

    }
    // Update is called once per frame
    void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu"); // Ganti dengan nama scene Main Menu Anda
    }

}
