using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBoundsCheck : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataManager.Instance.Character.Kill();
        }
    }
}
