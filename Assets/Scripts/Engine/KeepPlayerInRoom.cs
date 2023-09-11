using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class KeepPlayerInRoom : MonoBehaviour
{
    [Range(0f, 10f)]
    [SerializeField] private float PushForce = 5f;
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("IgnorePush"))
            return;

        Vector3 pushBack = new Vector3(0, 0, PushForce);
        other.attachedRigidbody.velocity = pushBack;
    }
}
