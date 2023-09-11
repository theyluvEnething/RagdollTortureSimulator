using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class ItemDynamite : Item
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _DynamitePrefab;
    [SerializeField] private ParticleSystem _explosionPrefab;

    [Header("IN SCENE SETTINGS")]
    [SerializeField] private Transform spawnpoint;

    [Header("EXPLOSION")]
    [SerializeField][Range(1f, 2000)] private int damage = 500;
    [SerializeField][Range(0f, 150f)] private float _explosionForce = 5f;
    [SerializeField][Range(0f, 30f)] private float _explosionRadius = 10f;

    private Animator _animator;
    private Rigidbody _rb;
    private Character _character;
    private Transform _ITEMGROUP;

    private bool thrown, finishedStart;


    Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction

    [SerializeField]
    float throwForceInXandY = 1f; // to control throw force in X and Y directions

    [SerializeField]
    float throwForceInZ = 50f; // to control throw force in Z direction


    IEnumerator Start()
    {
        _character = DataManager.Instance.Character;
        _ITEMGROUP = DataManager.Instance.ItemGroup;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _animator.enabled = true;

        yield return new WaitForSeconds(0.5f);

        _animator.enabled = false;
        finishedStart = true;
        InputManager.Instance._Dynamite = this.transform.gameObject;
    }
    void Update()
    {
        if (!finishedStart)
            return;

        if (thrown)
        {


            return;
        }

        Vector3 inputPosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            touchTimeStart = Time.time;
            startPos = inputPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().enabled = true;
            }
            Instantiate(_DynamitePrefab, spawnpoint.position, spawnpoint.rotation, _ITEMGROUP);
            thrown = true;

            touchTimeFinish = Time.time;
            timeInterval = touchTimeFinish - touchTimeStart;
            endPos = inputPosition;
            direction = startPos - endPos;

            _rb.isKinematic = false;
            Vector3 throwForce = new Vector3(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ * 10f);
            Debug.Log(throwForce);
            _rb.AddForce(throwForce);
            _rb.AddTorque(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ);
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(1.75f);
     
        Instantiate(_explosionPrefab, transform.position, transform.rotation);
        Collider[] surroundingObjects = Physics.OverlapSphere(transform.position, 20f);

        foreach (Collider collider in surroundingObjects)
        {
            if (collider.GetComponent<ItemDynamite>())
                continue;

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
            if (rb) rb.AddExplosionForce(50f, transform.position, 20f, 2f, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.75f);

        Destroy(gameObject);
    }
    private IEnumerator DelayedKnockback(Vector3 explosionPoint)
    {
        Debug.Log(explosionPoint);
        Collider[] surroundingObjects = Physics.OverlapSphere(explosionPoint, _explosionRadius);

        foreach (Collider collider in surroundingObjects)
        {
            if (collider.GetComponent<ItemDynamite>())
                continue;

            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb) rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 2f, ForceMode.Impulse);
        }
        yield return null;
    }

    /* MOBIBLE
    void Update()
    {

        // if you touch the screen
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {

            // getting touch position and marking time when you touch the screen
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }

        // if you release your finger
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {

            // marking time when you release it
            touchTimeFinish = Time.time;

            // calculate swipe time interval 
            timeInterval = touchTimeFinish - touchTimeStart;

            // getting release finger position
            endPos = Input.GetTouch(0).position;

            // calculating swipe direction in 2D space
            direction = startPos - endPos;

            // add force to balls rigidbody in 3D space depending on swipe time, direction and throw forces
            rb.isKinematic = false;
            rb.AddForce(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ / timeInterval);

            // Destroy ball in 4 seconds
            Destroy(gameObject, 3f);

        }

    }*/
}