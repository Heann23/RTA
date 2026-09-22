using System;
using UnityEngine;

public class NoteMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 200f;
    [SerializeField] private float topLaneSpacing = 70f;
    [SerializeField] private float bottomLaneSpacing = 120f;
    [SerializeField] private float spawnY = 300f;
    [SerializeField] private float endY = -80f;
    [SerializeField] private float startScale = 0.45f;
    [SerializeField] private float endScale = 1f;
    [SerializeField] private float hitDistance = 45f;

    private RectTransform _rectTransform;
    private RectTransform _battlePlayer;
    private BattleManager _battleManager;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private bool _isJudged;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(int lane, RectTransform battlePlayer, BattleManager battleManager)
    {
        _battlePlayer = battlePlayer;
        _battleManager = battleManager;
        
        int centeredLane = lane - 2;

        _startPosition = new Vector2(centeredLane * topLaneSpacing, spawnY);
        _targetPosition = new Vector2(centeredLane * bottomLaneSpacing, endY);

        _rectTransform.anchoredPosition = _startPosition;
        _rectTransform.localScale = Vector3.one * startScale;
    }

    private void Update()
    {
        _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, _targetPosition,
            moveSpeed * Time.deltaTime);

        float progress = Vector2.Distance(_startPosition, _rectTransform.anchoredPosition) /
                         Vector2.Distance(_startPosition, _targetPosition);
        float scale = Mathf.Lerp(startScale, endScale, progress);
        _rectTransform.localScale = Vector3.one * scale;

        if (!_isJudged && _rectTransform.anchoredPosition.y <= _battlePlayer.anchoredPosition.y) Judge();
        
        if (Vector2.Distance(_rectTransform.anchoredPosition, _targetPosition) < 0.1f) Destroy(gameObject);
    }

    private void Judge()
    {
        _isJudged = true;
        float distance = Mathf.Abs(_rectTransform.anchoredPosition.x - _battlePlayer.anchoredPosition.x);

        if (distance <= hitDistance) _battleManager.PlayerHit();
        else _battleManager.NoteDodged();
    }
}
