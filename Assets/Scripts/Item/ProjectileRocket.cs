using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRocket : MonoBehaviour
{

    [Header("ROCKET")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ParticleSystem _bulletImpact;
    [SerializeField] private ParticleSystem _bulletExplosion;
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField][Range(1f, 100f)] private float speed = 5f;

    [Header("EXPLOSION")]
    [SerializeField][Range(1f, 2000)] private int damage = 2000;
    [SerializeField][Range(0f, 150f)] private float _explosionForce = 10f;
    [SerializeField][Range(0f, 30f)] private float _explosionRadius = 12f;

    [Header("CHARACTER")]
    [SerializeField] private Character _character;
    [SerializeField] private RagdollEnabler _ragdollEnabler;

    void Start()
    {
        _bulletImpact.Pause();
        _character = DataManager.Instance.Character;
        _ragdollEnabler = DataManager.Instance.RagdollEnabler;

        if (!_rb) _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rb.velocity = transform.up * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(_bulletExplosion, transform.position, transform.rotation);
        Collider[] surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider collider in surroundingObjects)
        {
            if (collider.transform.CompareTag("EnableRagdoll"))
            {
                RagdollEnabler ragdollAnimator = collider.GetComponent<RagdollEnabler>();
                ragdollAnimator.EnableRagdoll();
                Character character = collider.GetComponent<Character>();
                character.Damage(damage);
                StartCoroutine(DelayedKnockback(transform.position));
            }
            if (collider.transform.CompareTag("Player"))
            {
                _character.Damage(damage / 12);
            }
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb) rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 2f, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
    private IEnumerator DelayedKnockback(Vector3 explosionPoint)
    {
        Debug.Log(explosionPoint);
        Collider[] surroundingObjects = Physics.OverlapSphere(explosionPoint, _explosionRadius);

        foreach (Collider collider in surroundingObjects)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb) rb.AddExplosionForce(_explosionForce, explosionPoint, _explosionRadius, 2f, ForceMode.Impulse);
        }
        yield return null;
    }
}

