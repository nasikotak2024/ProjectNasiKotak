using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Pengumuman : MonoBehaviour
{
    [SerializeField] private GameObject PengumumanCanvas;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] [TextArea] private string announcementText;
    [SerializeField] private float typingSpeed = 0.05f; // Kecepatan efek mengetik

    private bool playerInRange;
    private bool isTyping;
    private Coroutine typingCoroutine;

    void Start()
    {
        PengumumanCanvas.SetActive(false); // Menonaktifkan canvas saat mulai
    }

    void Update()
    {
        if (playerInRange && Input.GetButtonDown("IPengumuman"))
        {
            if (!PengumumanCanvas.activeInHierarchy)
            {
                PengumumanCanvas.SetActive(true);
                Time.timeScale = 0f; // Menghentikan waktu
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                typingCoroutine = StartCoroutine(TypeSentence(announcementText));
            }
            else if (isTyping)
            {
                // Menyelesaikan pengetikan segera jika tombol ditekan saat pengetikan berlangsung
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                dialogueText.text = announcementText;
                isTyping = false;
            }
            else
            {
                // Keluar dari dialog jika tombol ditekan setelah pengetikan selesai
                PengumumanCanvas.SetActive(false);
                Time.timeScale = 1f; // Mengaktifkan kembali waktu
            }
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        isTyping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            PengumumanCanvas.SetActive(false);
            Time.timeScale = 1f; // Mengaktifkan kembali waktu jika keluar dari area interaksi
        }
    }
}
