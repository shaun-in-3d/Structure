using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement3D : MonoBehaviour
{
    public float moveSpeed = 5f;
    // Adding a force multiplier to control the slipperiness more finely
    public float forceMultiplier = 10f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["Movement"].performed += OnMovement;
        playerInput.actions["Movement"].canceled += OnMovement;
    }
    
    private void OnDisable()
    {
        playerInput.actions["Movement"].performed -= OnMovement;
        playerInput.actions["Movement"].canceled -= OnMovement;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, 0f, input.y); // Convert 2D input to 3D
        GetComponent<PlayerSpriteSwitch>().ChangeDirection(moveInput);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Apply a force to move the player
        Vector3 force = moveInput * moveSpeed * forceMultiplier;
        rb.AddForce(force, ForceMode.Force);
        
    }
    
    
}