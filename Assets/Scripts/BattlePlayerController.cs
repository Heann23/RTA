using UnityEngine;
using UnityEngine.InputSystem;

public class BattlePlayerController : MonoBehaviour
{
    [SerializeField] private float laneSpacing = 120f;
    [SerializeField] private float moveSpeed = 12f;
    
    private RectTransform _rectTransform;
    private int _currentLane = 2;
    private Vector2 _targetPosition;
    
    public int CurrentLane => _currentLane;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        SetTargetPosition();
    }

    private void Update()
    {
        HandleInput();
        
        _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition, _targetPosition, moveSpeed * Time.deltaTime);
    }

    private void HandleInput()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
            MoveLane(-1);

        if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
            MoveLane(1);
    }

    private void MoveLane(int direction)
    {
        _currentLane = Mathf.Clamp(_currentLane + direction, 0, 4);
        SetTargetPosition();
    }

    private void SetTargetPosition()
    {
        float x = (_currentLane - 2) * laneSpacing;
        _targetPosition = new Vector2(x, _rectTransform.anchoredPosition.y);
    }
}
