using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ItemBazooka : Item
{
    [Header("BAZOOKA")]
    [SerializeField] private Item _Item;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private GameObject _rocket;
    [Header("CROSSHAIR")]
    [SerializeField] private Texture2D _crosshairTexture;
    [SerializeField] private float crosshairSize = 20f;

    private AudioSource _AudioSource;
    private Vector3 inputPosition;
    private Vector3 targetPosition;
    private Camera Camera;
    private int layerMask;
    private float shootTimer;

    void Start()
    {
        _Player = DataManager.Instance.Player;
        _PlayerTransform = DataManager.Instance.PlayerTransform;
        _Character = DataManager.Instance.Character;
        _Item = DataManager.Instance.Item;
        Camera = DataManager.Instance.MainCamera;
        _AudioSource = GetComponent<AudioSource>();
        layerMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Background");
        _CrosshairOffset = new Vector2(70, -45);
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && shootTimer <= 0)
        {
            _AudioSource.PlayOneShot(_AudioSource.clip);
            Instantiate(_muzzleFlash, _muzzlePoint);
            Instantiate(_rocket, _muzzlePoint.position, _muzzlePoint.rotation);
            shootTimer = 1.5f;
        }
    }
    void FixedUpdate()
    {
        shootTimer -= Time.deltaTime;

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
}
