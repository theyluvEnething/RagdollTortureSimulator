using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GarbageBinController : MonoBehaviour
{

    [SerializeField] ResetLevel levelReset;
    
    private Animator animator;
    private bool isOpen = false;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (!isOpen)
        {
            return;
        }

        ExitLevel();
    }

    void OnMouseEnter()
    {
        SetLidOpen(true);
    }

    void OnMouseExit()
    {
        SetLidOpen(false);
    }

    void SetLidOpen(bool open)
    {
        isOpen = open;
        animator.SetBool("IsOpen", open);
    }

    private void ExitLevel()
    {
        levelReset.resetLevel();
    }

    /*
    IEnumerator PullRigidbodies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 15F); // Change the radius to 10 units

        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject) // Don't apply force to the attractor object itself
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                Vector3 direction = transform.position - collider.transform.position;
                float distance = direction.magnitude;

                // Avoid division by zero
                if (distance < 0.001f)
                    distance = 0.001f;

                float forceMagnitude = gravitationalForce / (distance * distance);

                if (rb != null)
                {
                    rb.AddForce(direction.normalized * forceMagnitude, ForceMode.Acceleration);
                }
                else
                {
                    collider.transform.position += direction.normalized * forceMagnitude * Time.fixedDeltaTime;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    */
}
