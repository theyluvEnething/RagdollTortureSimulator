using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemAxe : Item
{
    [Header("REFERENCES")]
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Rigidbody _axehead;
    
    private Rigidbody rb;
    private float hitTimer;

    /*
    private float ChargeAnimation, ThrowAnimation;
    private bool chargingAxe, throwingAxe;
    */

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //if (rb) rb.centerOfMass = _centerOfMass.position;
        _Character = DataManager.Instance.Character;
    }
    void FixedUpdate()
    {
        hitTimer += Time.deltaTime;
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && hitTimer >= 0)
        {
            int damage = Mathf.Clamp((int) rb.velocity.magnitude, 200, 1000);
            damagePlayer(damage);
            hitTimer = -1f;
        }
    }
    */


    /*
    private void Update()
    {
        Debug.Log(ChargeAnimation);
        if (isDragged && ChargeAnimation <= 3f)
        {
            Debug.Log("Charging Axe");
            ChargeAnimation += Time.deltaTime;
            
            _axehead.velocity = (transform.position - _Character.transform.position);
           // StartCoroutine(ChargeThrow());
        }
        if (ChargeAnimation > 3)
        {
            InputManager.InputState inputState = InputManager.Instance.inputState;
            InputManager.Instance.InterruptDrag();
            throwingAxe = true;
        }
        if (ThrowAnimation <= 3f && throwingAxe)
        {
            ThrowAnimation += Time.deltaTime;
            _axehead.velocity = (transform.position - _Character.transform.position) * -1f;
        }
        if (ThrowAnimation > 3f && throwingAxe)
        {
            throwingAxe = false;
            ThrowAnimation = 0f;
            ChargeAnimation = 0f;
        }
    }
    */
}
