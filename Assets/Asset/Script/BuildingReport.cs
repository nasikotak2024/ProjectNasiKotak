using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BuildingReport : MonoBehaviour
{
    [SerializeField] private string nextScene; // Scene berikutnya
    [SerializeField] private GameObject dialogueCanvas; // Canvas untuk dialog
    [SerializeField] private TMP_Text dialogueText; // TextMeshPro untuk dialog
    [SerializeField] [TextArea] private string completedObjectiveDialogue; // Dialog saat objective terpenuhi
    [SerializeField] [TextArea] private string incompleteObjectiveDialogue; // Dialog saat objective belum terpenuhi

    private bool playerInRange = false;
    private bool dialogueActive = false;

    void Update()
    {
        if (playerInRange && Input.GetButtonDown("Interact"))
        {
            if (dialogueActive)
            {
                if (ObjectiveManager.instance.AreAllItemsCollected())
                {
                    SceneManager.LoadScene(nextScene); // Pindah ke scene berikutnya
                }
                else
                {
                    dialogueCanvas.SetActive(false);
                    dialogueActive = false;
                    Time.timeScale = 1f; // Resume game time

                }
            }
            else
            {
                ShowDialogue();

            }
        }
    }

    private void ShowDialogue()
    {
        dialogueCanvas.SetActive(true);
        if (ObjectiveManager.instance.AreAllItemsCollected())
        {
            dialogueText.text = completedObjectiveDialogue;

        }
        else
        {
            dialogueText.text = incompleteObjectiveDialogue;
        }
        dialogueActive = true;
        Time.timeScale = 0f; // Pause game time

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ObjectiveManager.instance.AreAllItemsCollected())
        {
            Time.timeScale = 1f; // Resume game time
        }
        else
        {
            playerInRange = false;
            dialogueCanvas.SetActive(false);
            dialogueActive = false;
            Time.timeScale = 1f; // Resume game time
        }
    }
}
