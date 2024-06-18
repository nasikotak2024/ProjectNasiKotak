using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    public List<string> itemNames; // Daftar nama item yang harus dikumpulkan
    private HashSet<string> itemsCollected = new HashSet<string>();

    public TMP_Text objectiveText; // UI Text di layar pause untuk menampilkan objective
    public GameObject objectiveUI; // UI panel untuk menampilkan objective terbaru

    public bool isPakBanuDialogCompleted = false; // Menyimpan status dialog dengan Pak Banu

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Membuat ObjectiveManager tidak hancur saat pergantian scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateObjectiveText();
    }

    public void CollectItem(string itemName)
    {
        if (!itemsCollected.Contains(itemName) && itemNames.Contains(itemName))
        {
            itemsCollected.Add(itemName);
            UpdateObjectiveText();
        }
    }

    private void UpdateObjectiveText()
    {
        objectiveText.text = "Objective:\n";
        foreach (string itemName in itemNames)
        {
            if (itemsCollected.Contains(itemName))
            {
                objectiveText.text += "<color=green>" + itemName + "</color>\n";
            }
            else
            {
                objectiveText.text += "<color=white>" + itemName + "</color>\n";
            }
        }
        objectiveUI.SetActive(true);
    }

    public bool AreAllItemsCollected()
    {
        return itemsCollected.Count == itemNames.Count;
    }

    public void AddObjective(string newObjective)
    {
        if (!itemNames.Contains(newObjective))
        {
            itemNames.Add(newObjective);
            UpdateObjectiveText();
        }
    }

    public void UpdateObjectiveUI()
    {
        UpdateObjectiveText();
    }

    public List<string> GetObjectives()
    {
        return itemNames;
    }

    public string GetLatestObjective()
    {
        return itemNames[itemNames.Count - 1];
    }

    public void CompletePakBanuDialog()
    {
        isPakBanuDialogCompleted = true;
        // Update objective jika diperlukan
        // Contoh: Tambahkan objective baru setelah berbicara dengan Pak Banu
        AddObjective("Pergi dan dapatkan data yang valid");
        UpdateObjectiveUI();
    }
}
