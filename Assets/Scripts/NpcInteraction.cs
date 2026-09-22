using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcInteraction : MonoBehaviour
{
    [Header("NPC 정보")] 
    [SerializeField] private string npcName = "NPC";
    [TextArea(2, 5)] 
    [SerializeField] private string[] dialogueLines;

    [Header("UI")] 
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("전투")] 
    [SerializeField] private BattleManager battleManager;
    
    private bool _playerInRange;
    private bool _isTalking;
    private bool _battleCleared;
    private int _currentLine;
    
    private PlayerController _playerController;

    private void Start()
    {
        interactionPrompt.SetActive(false);
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (battleManager != null && battleManager.IsBattleActive) return;
        if (!_playerInRange || Keyboard.current == null) return;
        if (!Keyboard.current.eKey.wasPressedThisFrame) return;
        
        if (!_isTalking) StartDialogue();
        else NextDialogue();
    }
    
    private void StartDialogue()
    {
        if (dialogueLines.Length == 0) return;

        _isTalking = true;
        _currentLine = 0;
        
        interactionPrompt.SetActive(false);
        dialoguePanel.SetActive(true);

        nameText.text = npcName;
        dialogueText.text = dialogueLines[_currentLine];

        if (_playerController != null) _playerController.SetCanMove(false);
    }

    private void NextDialogue()
    {
        _currentLine++;

        if (_currentLine >= dialogueLines.Length)
        {
            if (_battleCleared) EndDialogue();
            else StartBattle();
            
            return;
        }

        dialogueText.text = dialogueLines[_currentLine];
    }

    private void StartBattle()
    {
        _isTalking = false;
        
        interactionPrompt.SetActive(false);
        dialoguePanel.SetActive(false);
        
        battleManager.StartBattle(this);
    }

    public void OnBattleEnded(bool isVictory)
    {
        if (isVictory) _battleCleared = true;
        if (_playerInRange) interactionPrompt.SetActive(true);
    }

    private void EndDialogue()
    {
        _isTalking = false;
        dialoguePanel.SetActive(false);

        if (_playerController != null) _playerController.SetCanMove(true);
        
        if (_playerInRange) interactionPrompt.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerController = other.GetComponent<PlayerController>();
        _playerInRange = true;

        if (battleManager != null && battleManager.IsBattleActive) return;
        
        if (!_isTalking) interactionPrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        _playerInRange = false;
        
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (_isTalking)  EndDialogue();
    }
}
