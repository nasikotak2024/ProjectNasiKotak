using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{

    [SerializeField] private string GantiScene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ObjectiveManager.instance.isPakBanuDialogCompleted)
            {
                SceneManager.LoadScene("Desa"); // Pindah ke scene 1
            }
            else
            {
                SceneManager.LoadScene(GantiScene);
            }
        }

        
    }
}
