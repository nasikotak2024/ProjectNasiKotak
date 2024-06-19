using UnityEngine;

public class Signboard : MonoBehaviour
{
    [TextArea]
    public string[] tutorialMessages; // Array of messages to display on interaction
    public Sprite[] tutorialImages; // Array of images to display on interaction (optional)

    private bool isPlayerInRange;

 

    private void Update()
    {
        if (isPlayerInRange && Input.GetButtonDown("Interact"))
        {
            if (TutorialManager.instance.IsTutorialActive())
            {
                if (TutorialManager.instance.IsLastMessage())
                {
                    TutorialManager.instance.HideTutorial();
                    Time.timeScale = 1f; // Resume game time
                }
                else
                {
                    TutorialManager.instance.ShowNextMessage();
                }
            }
            else
            {
                TutorialManager.instance.StartTutorial(tutorialMessages, tutorialImages);
                Time.timeScale = 0f; // Pause game time
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (TutorialManager.instance.IsTutorialActive())
            {
                TutorialManager.instance.HideTutorial();
                Time.timeScale = 1f; // Resume game time
            }
        }
    }
}
