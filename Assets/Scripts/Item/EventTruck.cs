using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventTruck : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Character _character;
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private Transform _FrontCabine;

    private bool startMovement;
    IEnumerator Start()
    {
        if (!_rb) _rb = GetComponent<Rigidbody>();
        _character = DataManager.Instance.Character;

        yield return new WaitForSeconds(3.2f);
        startMovement = true;
    }
    void FixedUpdate()
    {
        if (!startMovement)
            return;

        _rb.velocity = Vector3.left * 100f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            _character.Kill();
            StartCoroutine(DelayExplosion());
        }
    }

    protected IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(_smoke, _FrontCabine.position, _FrontCabine.rotation);
    }
}
