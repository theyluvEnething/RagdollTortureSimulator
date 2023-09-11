using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemChainsaw : Item
{
    [SerializeField] private Transform _chainsawBlade;
    [SerializeField] private Character _character;
    [SerializeField] private ParticleSystem _sparks;

    private const float EFFECT_TIMER = 0.2f;
    private float effectTimer = EFFECT_TIMER;

    private void Start()
    {
        _character = DataManager.Instance.Character;
        _PlayerTransform = DataManager.Instance.PlayerTransform;
    }
    void FixedUpdate()
    {
        transform.LookAt(_PlayerTransform);
        effectTimer -= Time.deltaTime;
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player")) {
            _character.Damage(25);

            if (effectTimer <= 0)
            {
                Instantiate(_sparks, _chainsawBlade.position, _chainsawBlade.rotation);
                effectTimer = EFFECT_TIMER;
            }
        }
    }
}
