using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumping = false;

    private CustomInput input = null;
    private Vector2 moveVector = Vector2.zero;
        
    private PlayerInput playerInput;
    
    private void Awake()
    {
        input = new CustomInput();
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        int index = playerInput.playerIndex;
    }
    
    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.Jump.performed += OnJump; // Registering the jump action
    }
    
    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.Jump.performed -= OnJump; // Unregistering the jump action
    }
    

    public void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }
    
    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
    
    public void OnJump(InputAction.CallbackContext context) {
        if (context.performed && IsGrounded()) {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(moveVector);
        Move();
        
        if (isJumping) {
            Jump();
        }
    }
        
    
    
    private void Move()
    {
        
        float xForce = moveVector.x * moveSpeed; // Extract X from moveVector

        rb.AddForce(new Vector2(xForce, 0f), ForceMode2D.Force);        // Apply x force
    }


    private void Jump()
    {
        // Only allow the player to jump if they're on the ground
        if (IsGrounded())
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        isJumping = false; // Reset jump statuss until the next jump input
    }

    
 
    public float groundCheckRadius = 0.2f; // Radius of the circle used for ground detection
    public LayerMask groundLayer; // Layer used to identify the ground. Make sure to assign this in the Unity Editor.


    private bool IsGrounded() {
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