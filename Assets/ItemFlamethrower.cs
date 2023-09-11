using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ItemFlamethrower : Item
{
    [Header("FLAMETHROWER")]
    [SerializeField] private Item _Item;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private ParticleSystem _Flame;
    [SerializeField][Range(0f, 30f)] private float damage = 15f;

    [Header("CROSSHAIR")]
    [SerializeField] private Texture2D _crosshairTexture;
    [SerializeField] private float crosshairSize = 50f;

    private Vector3 inputPosition, targetPosition;
    private Camera Camera;
    private int layerMask;
    private float shootTimer;
    private bool shot;

    void Start()
    {
        _Player = DataManager.Instance.Player;
        _PlayerTransform = DataManager.Instance.PlayerTransform;
        _Character = DataManager.Instance.Character;
        _Item = DataManager.Instance.Item;
        _RagdollEnabler = DataManager.Instance.RagdollEnabler;
        Camera = DataManager.Instance.MainCamera;
        layerMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Background");
        _CrosshairOffset = new Vector2(70, -45);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && shootTimer <= 0)
        {
            Instantiate(_Flame, _muzzlePoint.position, _muzzlePoint.rotation);
            shootTimer = 0.05f;
            shot = true;
           
            if (GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().enabled = true;
            }
        }
    }

    void FixedUpdate()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer < -1f)
        {
            shot = false;

            if (GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().enabled = false;
            }
        }

        inputPosition = Input.mousePosition;
        inputPosition.x += _CrosshairOffset.x;
        inputPosition.y -= _CrosshairOffset.y;

        if (Physics.Raycast(Camera.ScreenPointToRay(inputPosition), out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            targetPosition = hit.point;
        }
        transform.LookAt(targetPosition);
    }

    private void OnGUI()
    {
        if (_crosshairTexture)
        {
            float x = Event.current.mousePosition.x + _CrosshairOffset.x;
            float y = Event.current.mousePosition.y + _CrosshairOffset.y;

            x -= crosshairSize / 2;
            y -= crosshairSize / 2;

            GUI.DrawTexture(new Rect(x, y, crosshairSize, crosshairSize), _crosshairTexture);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (!shot)
            return;

        if (other.transform.CompareTag("EnableRagdoll")) {
            _RagdollEnabler.EnableRagdoll();
            _Character.Damage((int)damage);
        }
        else if (other.transform.CompareTag("Player"))
        {
            _Character.Damage((int) damage);
        } 
    }
}
