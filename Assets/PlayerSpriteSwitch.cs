using UnityEngine;

public class PlayerSpriteSwitch : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Assign this in the Inspector
    public Sprite[] sprites; // Assign all your sprites in the Inspector

    public void ChangeDirection(Vector2 direction)
    {
        int index = DirectionToIndex(direction);
        spriteRenderer.sprite = sprites[index];
    }

    private int DirectionToIndex(Vector2 dir)
    {
        // Assuming sprites array is ordered as: Idle, N, NE, E, SE, S, SW, W, NW
        if (dir.magnitude < 0.01f)
            return 0; // Idle sprite

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        // Divide the circle into 8 directions + idle
        int index = Mathf.RoundToInt(angle / 45f) % 8 + 1; // +1 to skip the idle sprite

        return index;
    }
}