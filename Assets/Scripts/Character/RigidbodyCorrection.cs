using UnityEngine;

public class SetChildRigidbodyProperties : MonoBehaviour
{
    private void Awake()
    {
        SetChildRigidbodyPropertiesRecursively(transform);
    }

    private void SetChildRigidbodyPropertiesRecursively(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Check if the child has a Rigidbody component
            Rigidbody childRigidbody = child.GetComponent<Rigidbody>();
            if (childRigidbody != null)
            {
                // Set interpolation to Interpolate
                childRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                // Set collision detection to Continuous
                childRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            // Recursively check the child's children
            SetChildRigidbodyPropertiesRecursively(child);
        }
    }
}