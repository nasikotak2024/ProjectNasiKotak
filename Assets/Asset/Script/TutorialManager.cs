using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [SerializeField] private GameObject tutorialCanvas; // Canvas for displaying the tutorial
    [SerializeField] private TMP_Text tutorialText; // Text for displaying the tutorial message
    [SerializeField] private Image tutorialImage; // Image for displaying the tutorial image (optional)

    private string[] messages;
    private Sprite[] images;
    private int currentIndex;
    private bool tutorialActive = false;

    private void Start()
    {
        tutorialCanvas.SetActive(false);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartTutorial(string[] messages, Sprite[] images = null)
    {
        this.messages = messages;
        this.images = images;
        currentIndex = 0;
        ShowMessage();
        tutorialCanvas.SetActive(true);
        tutorialActive = true;
        Time.timeScale = 0f; // Pause game time
    }

    public void ShowNextMessage()
    {
        if (currentIndex < messages.Length - 1)
        {
            currentIndex++;
            ShowMessage();
        }
        else
        {
            HideTutorial();
        }
    }

    private void ShowMessage()
    {
        tutorialText.text = messages[currentIndex];
        if (images != null && images.Length > currentIndex && images[currentIndex] != null)
        {
            tutorialImage.sprite = images[currentIndex];
            tutorialImage.gameObject.SetActive(true);
        }
        else
        {
            tutorialImage.gameObject.SetActive(false);
        }
    }

    public void HideTutorial()
    {
        tutorialCanvas.SetActive(false);
        tutorialActive = false;
        Time.timeScale = 1f; // Resume game time
    }

    public bool IsTutorialActive()
    {
        return tutorialActive;
    }

    public bool IsLastMessage()
    {
        return currentIndex >= messages.Length - 1;
    }
}
