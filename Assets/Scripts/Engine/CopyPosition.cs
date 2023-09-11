using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] private Transform PositionToCopy;
    void FixedUpdate()
    {
        if (PositionToCopy)
        {
            transform.position = PositionToCopy.position;
            transform.rotation = PositionToCopy.rotation;
        }
    }
}
