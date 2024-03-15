using UnityEngine;

public class ParticleDirection : MonoBehaviour
{
    private PlayerSpriteSwitch playerSpriteSwitch;
    private Rigidbody rb;
    public ParticleSystem particles; // Assign this in the Inspector

    void Awake()
    {
        playerSpriteSwitch = GetComponentInParent<PlayerSpriteSwitch>();
        rb = playerSpriteSwitch.GetComponent<Rigidbody>();
    
        // Find the ParticleSystem component in the children of this GameObject.
        particles = GetComponentInChildren<ParticleSystem>();
    
        
    }


    void Update()
    {
        RotateParticleParent();
        AdjustParticleEmissionBasedOnSpeed();
    }

    void RotateParticleParent()
    {
        Vector2 movementDirection = new Vector2(rb.velocity.x, rb.velocity.z);

        if (movementDirection.magnitude > 0.1f)
        {
            int index = playerSpriteSwitch.DirectionToIndex(movementDirection.normalized);
            int angle = (index - 1) * 45;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        // Optionally, handle the idle state (index 0) if needed
    }

    void AdjustParticleEmissionBasedOnSpeed()
    {
        // Calculate the player's speed
        float speed = rb.velocity.magnitude;

        // Get the emission module from the particle system
        var emission = particles.emission;

        // Adjust the rate of emission based on speed. You may need to scale the speed to get a suitable rate.
        // For example, this could be a direct assignment, or you might use a formula to adjust the rate more subtly.
        emission.rateOverTime = speed * 10; // Example scaling factor, adjust based on your game's needs
    }
}