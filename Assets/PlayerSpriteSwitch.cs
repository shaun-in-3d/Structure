using UnityEngine;

public class PlayerSpriteSwitch : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Assign this in the Inspector.
    public Sprite[] sprites; // Assign all your sprites in the Inspector, ordered as Idle, N, NE, E, SE, S, SW, W, NW.

    private Rigidbody rb; // Use a Rigidbody for 3D physics.

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Convert the Rigidbody's 3D velocity to a 2D vector for sprite direction calculation.
        Vector2 movementDirection = new Vector2(rb.velocity.x, rb.velocity.z);

        if (movementDirection.magnitude > 0.1f)
        {
            // If there's significant movement, update the sprite direction.
            ChangeDirection(movementDirection.normalized);
        }
        else
        {
            // If movement is negligible, ensure the sprite is set to Idle.
            ChangeDirection(Vector2.zero);
        }
    }

    public void ChangeDirection(Vector2 direction)
    {
        // Calculate the index based on direction vector.
        int index = DirectionToIndex(direction);
        // Update the sprite based on the calculated index.
        spriteRenderer.sprite = sprites[index];
    }

    private int DirectionToIndex(Vector2 dir)
{
    if (dir.magnitude < 0.01f)
        return 0; // Return idle sprite if there's practically no movement.

    // Calculate the angle from the direction vector, making right (East) as 0 degrees.
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    if (angle < 0) angle += 360;

    // The order now matches a logical compass direction, so we can map directly.
    // Adjusting the angle to start from the "Up" (North) direction, which is -90 degrees from the right.
    angle = (angle + 90) % 360;
    // Mapping the angle to the sprite index, where 0 is idle, 1-8 are directions.
    int index = Mathf.RoundToInt(angle / 45f) % 8 + 1;

    return index;
}

}
