using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraaction : MonoBehaviour
{
    [Header("NPC 정보")] [SerializeField] private string npcName = "NPC";
    [TextArea(2, 5)] [SerializeField] private string[] dialogueLines;

    [Header("UI")] [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    private bool playerInrange;
    private bool isTalking;
    private int currentLine;
    private PlayerController playerController;

    private void Start()
    {
        interactionPrompt.SetActive(false);
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!playerInrange || Keyboard.current == null) return;
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isTalking) StartDialogue();
            else NextDialogue();
        }
    }
    
    private void StartDialogue()
    {
        if (dialogueLines.Length == 0) return;

        isTalking = true;
        currentLine = 0;
        
        interactionPrompt.SetActive(false);
        dialoguePanel.SetActive(true);

        nameText.text = npcName;
        dialogueText.text = dialogueLines[currentLine];

        if (playerController != null) playerController.SetCanMove(false);
    }

    private void NextDialogue()
    {
        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogueLines[currentLine];
    }

    private void EndDialogue()
    {
        isTalking = false;
        dialoguePanel.SetActive(false);

        if (playerController != null) playerController.SetCanMove(true);
        
        if (playerInrange) interactionPrompt.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerController = other.GetComponent<PlayerController>();
        playerInrange = true;
        
        if (!isTalking) interactionPrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        playerInrange = false;
        interactionPrompt.SetActive(false);
        
        if (isTalking)  EndDialogue();
    }
}
