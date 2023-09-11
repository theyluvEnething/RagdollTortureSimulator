using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Tarodev {
    
    public class ItemMissile : Item {

        [Header("REFERENCES")] 
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private ParticleSystem _smoke;
        //[SerializeField] private Character _character;

        [Header("MOVEMENT")] 
        [SerializeField] private float _speed = 15;
        [SerializeField] private float _rotateSpeed = 95;

        [Header("EXPLOSION")]
        [Range(0f, 150f)][SerializeField] private float _explosionForce = 5f;
        [Range(0f, 30f)][SerializeField] private float _explosionRadius = 10f;
        [Range(0f, 2500f)][SerializeField] private int _damage = 1250;

        [HideInInspector] private bool _startMissile = false;

        IEnumerator Start()
        {
            _target = DataManager.Instance.PlayerTransform;
            _Character = DataManager.Instance.Character;

            yield return new WaitForSeconds(0.3f);
            _startMissile = true;
        }

        private void FixedUpdate() {
            if (!_startMissile)
            {
                return;
            }
            _rb.velocity = transform.forward * _speed;
            RotateRocket();
            _rotateSpeed += 1f;
        }
   
        private void RotateRocket() {
            var heading = _target.position - transform.position;

            var rotation = Quaternion.LookRotation(heading);
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * 0.02f));
        }

        private void OnCollisionEnter(Collision collision) {
            if (_explosionPrefab) Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            if (_smoke) _smoke.Stop();

            Collider[] surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);

            foreach (Collider collider in surroundingObjects)
            {
                if (collider.transform.CompareTag("EnableRagdoll"))
                {
                    RagdollEnabler ragdollAnimator = collider.GetComponent<RagdollEnabler>();
                    ragdollAnimator.EnableRagdoll();
                    _Character.Damage(_damage);
                }
                if (collider.transform.CompareTag("Player"))
                {
                    _Character.Damage(_damage / 12);
                }
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                ItemMissile otherMissile = collider.GetComponent<ItemMissile>();
                if (rb && !otherMissile) rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 2f, ForceMode.Impulse);
            }

            Destroy(gameObject);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}