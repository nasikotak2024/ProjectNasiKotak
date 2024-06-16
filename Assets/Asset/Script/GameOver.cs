using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Image gameOverImage;
    public AudioSource gameOverAudio;


    void Update()
    {
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            GameOver();
        }
    }

    // Game over function
    private void GameOver()
    {
        if (gameOverImage != null)
        {
            gameOverImage.gameObject.SetActive(true);
        }

        if (gameOverAudio != null)
        {
            gameOverAudio.Play();
        }

        Time.timeScale = 0f;
    }

}
