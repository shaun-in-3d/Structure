using UnityEngine;

public class PlayerSpriteSwitch : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Assign this in the Inspector.
    public Sprite[] sprites; // Assign all your sprites in the Inspector, ordered as Idle, N, NE, E, SE, S, SW, W, NW.
    public AK.Wwise.Event cyclingSound = new AK.Wwise.Event(); // The Wwise cycling sound event.
    public AK.Wwise.RTPC cyclingVolumeRTPC; // The RTPC for controlling the cycling sound volume.

    private Rigidbody rb; // Use a Rigidbody for 3D physics.
    private bool isCyclingSoundPlaying = false; // Tracks if the cycling sound has been started.

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Optionally start the cycling sound here and only control its volume later.
        cyclingSound.Post(gameObject);
        isCyclingSoundPlaying = true;
    }

    private void Update()
    {
        Vector2 movementDirection = new Vector2(rb.velocity.x, rb.velocity.z);

        if (movementDirection.magnitude > 0.1f)
        {
            ChangeDirection(movementDirection.normalized);
            // Ensure cycling sound volume is set to normal (100) if it was previously turned down.
            if (isCyclingSoundPlaying)
            {
                cyclingVolumeRTPC.SetGlobalValue(100); // Assuming 100 is the full volume.
            }
        }
        else
        {
            ChangeDirection(Vector2.zero);
            // Reduce cycling sound volume to 0.
            cyclingVolumeRTPC.SetGlobalValue(0);
        }
    }

    public void ChangeDirection(Vector2 direction)
    {
        int index = DirectionToIndex(direction);
        spriteRenderer.sprite = sprites[index];
    }

    public int DirectionToIndex(Vector2 dir)
    {
        if (dir.magnitude < 0.01f)
            return 0;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        angle = (angle + 90) % 360;
        int index = Mathf.RoundToInt(angle / 45f) % 8 + 1;

        return index;
    }
}
