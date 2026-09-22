using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D _rb;
    private float _moveInput;
    private bool _canMove = true;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_canMove)
        {
            _moveInput = 0f;
            return;
        }

        _moveInput = 0f;
        
        if (Keyboard.current == null) return;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) _moveInput -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) _moveInput += 1f;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_moveInput * moveSpeed, _rb.linearVelocity.y);
    }
    
    public void SetCanMove(bool value)
    {
        _canMove = value;

        if (!_canMove)
        {
            _moveInput = 0f;
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
        }
    }
}