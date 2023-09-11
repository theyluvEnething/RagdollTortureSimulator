using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageRagdollOnImpact : MonoBehaviour
{

    [Header("REFERENCES")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Character _character;
    [SerializeField] private ParticleSystem _impact;
    [SerializeField] private ParticleSystem _BloodSplash;

    private const float COLLISION_TIME = 1f;
    private const float MIN_DAMAGE = 500f;
    private const float MAX_DAMAGE = 2500f;
    private static float damageMultiplier = 50f;
    private static float collisionTimer = COLLISION_TIME;

    private static float bloodTimer = 0.2f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _character = DataManager.Instance.Character;
    }
    void Update()
    {
        bloodTimer -= Time.deltaTime;
        collisionTimer -= Time.deltaTime;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collisionTimer > 0)
            return;

        if (_rb.velocity.magnitude > 9f && collisionCheck(collision))
        {
            int damage = (int)Mathf.Clamp((_rb.GetComponent<Rigidbody>().velocity.magnitude * damageMultiplier), MIN_DAMAGE, MAX_DAMAGE);
            _character.Damage(damage);
            collisionTimer = COLLISION_TIME;
            Instantiate(_impact, _rb.transform.position, _rb.transform.rotation);

            if (GameManager.Instance.GetBloodUnlocked())
                SpawnBlood();
        }
        else if (collision.rigidbody)
        {
            if (collision.rigidbody.velocity.magnitude < 8.5f || !collisionCheck(collision))
                return;

            int damage = (int)Mathf.Clamp((collision.rigidbody.velocity.magnitude * damageMultiplier * 0.4f * collision.rigidbody.mass), MIN_DAMAGE, Mathf.Infinity);
            _character.Damage(damage);
            collisionTimer = COLLISION_TIME;
            Instantiate(_impact, _rb.transform.position, _rb.transform.rotation);

            if (GameManager.Instance.GetBloodUnlocked())
                SpawnBlood();
        }
        // EXPERIMENTALLL !!
        /* 
        else if (InputManager.isDragged && _rb.velocity.magnitude > 25f)
        {
            int damage = (int)Mathf.Clamp((_rb.velocity.magnitude * damageMultiplier * 0.5f), MIN_DAMAGE, Mathf.Infinity);
            _character.addDamage(damage);
            collisionTimer = COLLISION_TIME;
            Instantiate(_impact, _rb.transform.position, _rb.transform.rotation);
        }
        */
    }
    private bool collisionCheck(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
            return false;

        return true;
    }
    private void SpawnBlood()
    {
        if (bloodTimer <= 0)
        {
            Instantiate(_BloodSplash, transform);
            bloodTimer = 1f;
        }
    }
}
