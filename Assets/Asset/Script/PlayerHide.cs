using UnityEngine;
using System.Collections;

public class PlayerHide : MonoBehaviour
{
    public bool isHidden = false;
    public GameObject currentBarrel;
    private Topdown playerMovement;
    private AdvancedAIChaseAndReturn2D aiScript;

    void Start()
    {
        playerMovement = GetComponent<Topdown>();
        aiScript = FindObjectOfType<AdvancedAIChaseAndReturn2D>(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentBarrel != null)
        {
            if (isHidden)
            {
                ExitBarrel();
            }
            else
            {
                EnterBarrel();
            }
        }
    }

    void EnterBarrel()
    {
        isHidden = true;
        SetPlayerVisibility(false);
        playerMovement.enabled = false;
        if (aiScript != null)
        {
            aiScript.PlayerHiding(true);
        }
    }

    void ExitBarrel()
    {
        isHidden = false;
        SetPlayerVisibility(true);
        playerMovement.enabled = true;
        if (aiScript != null)
        {
            aiScript.PlayerHiding(false);
        }
    }

    public void SetPlayerVisibility(bool isVisible)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = isVisible;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            currentBarrel = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            if (isHidden)
            {
                ExitBarrel();
            }
            currentBarrel = null;
        }
    }

    public bool IsHidden()
    {
        return isHidden;
    }
}
