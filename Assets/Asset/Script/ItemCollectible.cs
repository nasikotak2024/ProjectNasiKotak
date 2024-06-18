using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Untuk menggunakan SceneManager
using System.Collections;
public class CollectibleItem : MonoBehaviour
{
    public string itemName; // Nama item yang akan dikumpulkan

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ObjectiveManager.instance.CollectItem(itemName);
            Destroy(gameObject);
        }
    }
}
