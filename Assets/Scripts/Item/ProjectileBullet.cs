using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    [Header("BULLET")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ParticleSystem _bulletImpact;
    [SerializeField][Range(1f, 100f)] private float speed = 5f;
    [SerializeField][Range(1f, 10f)] private float knockback = 5f;
    [SerializeField][Range(1f, 1000)] private int damage = 500;

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
        _rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("EnableRagdoll"))
        {
            _ragdollEnabler.EnableRagdoll();
            StartCoroutine(DelayedKnockback(collision, _rb.velocity));
        }
        if (!collision.transform.CompareTag("Player"))
        {
            Instantiate(_bulletImpact, transform.position, Quaternion.identity);
        }
        if (collision.rigidbody && !collision.transform.CompareTag("Player"))
        {
            collision.rigidbody.AddForce(_rb.velocity * knockback * 0.1f, ForceMode.Impulse);
            Destroy(gameObject);
        }
        if (collision.rigidbody && collision.transform.CompareTag("Player")) {
            _character.Damage(damage);
            collision.rigidbody.AddForce(_rb.velocity * knockback, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
    private IEnumerator DelayedKnockback(Collision collision, Vector3 force)
    {
        collision.rigidbody.AddForce(force * knockback, ForceMode.Impulse);
        yield return null;
    }
}
