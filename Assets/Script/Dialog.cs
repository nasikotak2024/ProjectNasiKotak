using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image portraitImage;
    [SerializeField] private string[] speaker;
    [SerializeField] [TextArea] private string[] dialogueWords;
    [SerializeField] private Sprite[] portrait;
    [SerializeField] private float typingSpeed = 0.05f; // Kecepatan efek mengetik

    private bool dialogueActivated;
    private int step;
    private Coroutine typingCoroutine;

    void Start()
    {
        dialogueText.text = "";
        dialogueCanvas.SetActive(false); // Menonaktifkan canvas saat mulai
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && dialogueActivated)
        {
            if (step >= speaker.Length)
            {
                dialogueCanvas.SetActive(false);
                step = 0;
                Time.timeScale = 1f; // Mengaktifkan kembali waktu
            }
            else
            {
                dialogueCanvas.SetActive(true);
                speakerText.text = speaker[step];
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                typingCoroutine = StartCoroutine(TypeSentence(dialogueWords[step]));
                portraitImage.sprite = portrait[step];
                Time.timeScale = 0f; // Menghentikan waktu
                step++;
            }
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    // Ketika objek memasuki trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogueActivated = true;
            // Mulai dengan dialog pertama
            step = 0;       
        }
    }

    // Ketika objek meninggalkan trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogueActivated = false;
            // Menonaktifkan canvas dialog
            dialogueCanvas.SetActive(false);
            // Mengatur langkah kembali ke 0 saat keluar dari trigger
            step = 0;        
        }
    }
}
