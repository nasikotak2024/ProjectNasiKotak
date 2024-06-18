using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public Text displayText; // UI Text komponen
    public Image displayImage; // UI Image komponen
    public float typingSpeed = 0.05f; // Kecepatan mengetik
    public string[] sentences; // Array kalimat
    private int index; // Indeks kalimat saat ini
    private bool isTyping = false;
    private bool cancelTyping = false;
    [SerializeField] private string GantiScene;


    void Start()
    {
        displayImage.gameObject.SetActive(false); // Sembunyikan gambar saat mulai
        StartCoroutine(TypeSentence());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                cancelTyping = true;
            }
            else
            {
                if (index < sentences.Length - 1)
                {
                    index++;
                    StartCoroutine(TypeSentence());
                }
                else
                {
                    // Ganti ke scene berikutnya
                    UnityEngine.SceneManagement.SceneManager.LoadScene(GantiScene);
                }
            }
        }
    }

    private IEnumerator TypeSentence()
    {
        isTyping = true;
        displayText.text = "";
        displayImage.gameObject.SetActive(true); // Tampilkan gambar

        foreach (char letter in sentences[index].ToCharArray())
        {
            if (cancelTyping)
            {
                displayText.text = sentences[index];
                cancelTyping = false;
                break;
            }
            displayText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
