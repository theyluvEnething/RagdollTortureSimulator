using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using PrimeTween;

public class ItemTruck : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Collider _Collider;
    [SerializeField] private GameObject _RightWall;
    [SerializeField] private GameObject _TruckObject;
    [SerializeField] private Transform _SpawnpointEventRoom;

    [Header("ATTRIBUTES")]
    [SerializeField] private AnimationCurve _OpenCurve;

    private bool coroutineRunning, animationRunning;
    private int itemLayerMask;
    private Camera Camera;


    void Start()
    {
        Camera = DataManager.Instance.MainCamera;
        itemLayerMask = 1 << LayerMask.NameToLayer("Item");

        if (!_Collider && !GetComponent<Collider>())
        {
            _Collider = GetComponent<Collider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (coroutineRunning || animationRunning)
            return;

        Vector3 inputPosition = Input.mousePosition;
        if (!Physics.Raycast(Camera.ScreenPointToRay(inputPosition), out RaycastHit hit, Mathf.Infinity, itemLayerMask))
            return;

        if (hit.transform.CompareTag("ItemTruck")) {
            Debug.Log("Clicked on Truck");
            animationRunning = true;
            InputManager.Instance.stopTutorialTimer = 10f;
            Tween.PositionX(transform, -10, 1.5f, Ease.OutQuad);
            StartCoroutine(TruckAnimation());
        }
    }
    private IEnumerator TruckAnimation()
    {
        coroutineRunning = true;

        Instantiate(_TruckObject, _SpawnpointEventRoom.position, _SpawnpointEventRoom.rotation);  
        Tween.PositionY(_RightWall.transform, -8, 1.5f, Ease.OutSine);
        Tween.ShakeCamera(Camera, 1);

        coroutineRunning = false;
        yield return null;
    }
}
