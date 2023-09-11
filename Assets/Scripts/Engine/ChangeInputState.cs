using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChangeInputState : MonoBehaviour
{
    [SerializeField] private GameManager.Item _ItemState;

    private LayerMask shopLayerMask;
    private Vector3 inputPosition;
    private Collider _Collider;
    private Camera _Camera;

    void Start()
    {
        _Camera = DataManager.Instance.MainCamera;

        if (GetComponent<Collider>())
            _Collider = GetComponent<Collider>();
    }
    void FixedUpdate()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        inputPosition = Input.mousePosition;
        if (!Physics.Raycast(_Camera.ScreenPointToRay(inputPosition), out RaycastHit hit, Mathf.Infinity))
            return;

        if (hit.collider == _Collider)
        GameManager.Instance.ItemState = _ItemState;
    }
}
