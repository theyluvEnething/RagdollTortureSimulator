using cakeslice;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.TextCore.Text;

public class PiranhaController : Item
{
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _target;

    [Header("ATTRIBUTES")]
    [SerializeField] private float _speed = 50;
    [SerializeField] private float _rotateSpeed = 95;

    private bool startPiranha = false;
    private static bool attractAllPiranhas = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _target = DataManager.Instance.PlayerTransform;
        _Character = DataManager.Instance.Character;
        
        startPiranha = false;
        attractAllPiranhas = false;
    }

    void FixedUpdate()
    {
        if (ChangeScene.Instance.isActiveAquarium && DataManager.Instance.RagdollAnimator._currentState == RagdollAnimator.RagdollState.Ragdoll)
            startPiranha = true;

        if (!startPiranha)
            return;

        if (Vector3.Distance(_target.position, transform.position) < 4f || attractAllPiranhas)
        { 
            attractAllPiranhas = true;
            _rb.velocity = transform.forward * _speed;
            var heading = _target.position - transform.position;
            var rotation = Quaternion.LookRotation(heading);
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (GetComponent<AudioSource>())
                GetComponent<AudioSource>().enabled = true;

            _Character.Damage(50, true);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (GetComponent<AudioSource>())
                GetComponent<AudioSource>().enabled = true;

            _Character.Damage(50, true);
        }
    }
}
