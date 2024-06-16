using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    public List<string> itemNames; // Daftar nama item yang harus dikumpulkan
    private HashSet<string> itemsCollected = new HashSet<string>();

    public TMP_Text objectiveText; // UI Text di layar pause untuk menampilkan objective

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
    }
}
