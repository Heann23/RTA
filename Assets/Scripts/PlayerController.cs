using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool canMove = true;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!canMove)
        {
            moveInput = 0f;
            return;
        }

        moveInput = 0f;
        
        if (Keyboard.current == null) return;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput += 1f;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
    
    public void SetCanMove(bool value)
    {
        canMove = value;

        if (!canMove)
        {
            moveInput = 0f;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }
}