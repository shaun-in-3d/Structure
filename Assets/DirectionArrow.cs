using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public Transform targetObject; // Assign this in the Inspector

    void Update()
    {
        if (targetObject)
        {
            // Find the target direction relative to the arrow
            Vector3 targetDirection = targetObject.position - transform.position;
            targetDirection.y = 0; // Keep the arrow's tilt unchanged

            // Determine the rotation to look at the target direction
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

            // Update the arrow's rotation towards the target direction
            // Only applying Y component of the quaternion to rotate around Y-axis
            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }
}