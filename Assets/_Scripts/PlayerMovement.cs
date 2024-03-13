using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    private Rigidbody2D rb;
    private Vector2 moveVector;
    private bool isJumping = false;

    // No need for a custom input class variable anymore.
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Input System's PlayerInput component handles enabling/disabling input,
    // so OnEnable and OnDisable are not needed for input handling anymore.

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
        if (isJumping)
        {
            Jump();
        }
    }

    private void Move()
    {
        // Apply movement
        float xForce = moveVector.x * moveSpeed;
        rb.velocity = new Vector2(xForce, rb.velocity.y); // Changed to set velocity directly for more consistent movement
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        isJumping = false; // Reset jumping state
    }

    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool IsGrounded()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y - groundCheckRadius);
        Collider2D hit = Physics2D.OverlapCircle(position, groundCheckRadius, groundLayer);
        return hit != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 position = new Vector2(transform.position.x, transform.position.y - groundCheckRadius);
        Gizmos.DrawWireSphere(position, groundCheckRadius);
    }
}
