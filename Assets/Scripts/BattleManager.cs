using System.Collections;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("전투 UI")]
    [SerializeField] private GameObject battlePanel;
    [SerializeField] private GameObject battleTitle;
    [SerializeField] private TMP_Text enemyHpText;
    [SerializeField] private TMP_Text playerHpText;
    [SerializeField] private TMP_Text battleResultText;
    

    [Header("플레이어")] 
    [SerializeField] private PlayerController playerController;
    
    [Header("노트")]
    [SerializeField] private NoteSpawner noteSpawner;

    [Header("체력")] 
    [SerializeField] private int maxEnemyHp = 100;
    [SerializeField] private int maxPlayerHp = 100;
    [SerializeField] private int dodgeDamage = 10;
    [SerializeField] private int hitDamage = 20;
    
    [Header("설정")] 
    [SerializeField] private float battleTitleDuration = 1.5f;
    [SerializeField] private float battleResultDuration = 2;
    

    private bool _isBattleActive;
    private int _enemyHp;
    private int _playerHp;
    private NpcInteraction _currentNpc;
    public bool IsBattleActive => _isBattleActive;

    private void Start()
    {
        battlePanel.SetActive(false);
        battleTitle.SetActive(false);
        battleResultText.gameObject.SetActive(false);
    }

    public void StartBattle(NpcInteraction npc)
    {
        if (_isBattleActive) return;
        
        _currentNpc = npc;
        _isBattleActive = true;

        _enemyHp = maxEnemyHp;
        _playerHp = maxPlayerHp;
        UpdateHpUi();
        
        battlePanel.SetActive(true);
        battleTitle.SetActive(true);
        battleResultText.gameObject.SetActive(false);
        
        noteSpawner.StartSpawning();
        
        if (playerController != null) playerController.SetCanMove(false);

        StartCoroutine(HideBattleTitle());
    }

    public void PlayerHit()
    {
        if (!_isBattleActive) return;
        
        _playerHp = Mathf.Max(0, _playerHp - hitDamage);
        UpdateHpUi();

        if (_playerHp <= 0) FinishBattle(false);
    }

    public void NoteDodged()
    {
        if (!_isBattleActive) return;
        
        _enemyHp = Mathf.Max(0, _enemyHp - dodgeDamage);
        UpdateHpUi();

        if (_enemyHp <= 0) FinishBattle(true);
    }

    private void UpdateHpUi()
    {
        enemyHpText.text = $"Boss HP : {_enemyHp} / {maxEnemyHp}";
        playerHpText.text = $"Player HP : {_playerHp} / {maxPlayerHp}";
    }

    private void FinishBattle(bool isVictory)
    {
        if (!_isBattleActive) return;
        
        noteSpawner.StopSpawning();

        battleResultText.text = isVictory ? "승리" : "패배";
        battleResultText.gameObject.SetActive(true);

        StartCoroutine(EndBattle(isVictory));
    }

    private IEnumerator EndBattle(bool isVictory)
    {
        yield return new WaitForSeconds(battleResultDuration);
        
        battleResultText.gameObject.SetActive(false);
        battlePanel.SetActive(false);

        _isBattleActive = false;

        if (playerController != null) playerController.SetCanMove(true);
        if (_currentNpc != null) _currentNpc.OnBattleEnded(isVictory);
        
        _currentNpc = null;
    }

    private IEnumerator HideBattleTitle()
    {
        yield return new WaitForSeconds(battleTitleDuration);
        battleTitle.SetActive(false);
    }
}
