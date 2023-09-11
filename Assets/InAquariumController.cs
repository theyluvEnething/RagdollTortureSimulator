using Piranha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAquariumController : MonoBehaviour
{
    private float oldDrag;
    private float oldAngulardrag;
    private bool once = false;
    void OnTriggerStay(Collider collision)
    {
        if (collision.attachedRigidbody)
        {
            Rigidbody _rb = collision.attachedRigidbody;
            if (!once)
            {
                oldDrag = _rb.drag;
                oldAngulardrag = _rb.angularDrag;
                once = true;
            }

            _rb.drag = 5.5f;
            _rb.angularDrag = 10f;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.attachedRigidbody) 
        {
            collision.attachedRigidbody.AddForce(Vector3.down * 10f, ForceMode.Acceleration);
            //Rigidbody _rb = collision.attachedRigidbody;
            //once = false;
            //_rb.drag = oldDrag;
            //_rb.angularDrag = oldAngulardrag;
        }
    }
}
