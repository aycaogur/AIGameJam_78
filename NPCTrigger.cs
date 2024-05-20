using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTrigger : MonoBehaviour
{
    public ObjectDialogue dialogue;
    public Button interactButton;

    private void Awake()
    {
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Interact button is not assigned in the inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("npc"))
        {
            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(true);
                Debug.Log("Button activated");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("npc"))
        {
            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(false);
                Debug.Log("Button deactivated");
            }
        }
    }

    public void TriggerDialogue()
    {
        FindAnyObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}
