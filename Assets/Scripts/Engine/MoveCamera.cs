using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Collider))]
public class MoveCamera : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform TargetPoint;
    [SerializeField] private Vector3 _StartPosition; 
    [SerializeField] private Vector3 _StartRotation;

    [Header("SETTINGS")]
    [SerializeField] private AnimationCurve _Curve;
    [SerializeField] private bool _ExitSignOnScreen = false;
    [SerializeField] private bool _ColliderEnabled = false;
    [SerializeField] private bool _OnlyExitSign = false;
    [SerializeField] private ChangeScene.SceneState _sceneState;

    [SerializeField] public static bool CameraIsMoving = false;
    [SerializeField] public static bool allowOnlyExitSign = false;

    private bool opened, finishedRotating, finishedMoving;
    private Collider _Collider;
    private Camera Camera;
    private float factor;
    private int exitSignLayerMask;

    void Start()
    {
        Camera = DataManager.Instance.MainCamera;
        if (GetComponent<Collider>()) _Collider = GetComponent<Collider>();
        exitSignLayerMask = 1 << LayerMask.NameToLayer("Exit Sign");

        if (_OnlyExitSign && !_ExitSignOnScreen)
        {
            Debug.LogError("Cannot leave this camera scene because there is no exit sign but only exit sign is allowed.");
        }
    }
    void FixedUpdate()
    {
        if (Character.Instance.isDead)
            return;

        checkExit();
        onTouchDown();
        SmoothFollow();
    }
    private void SmoothFollow()
    {
        if (!opened) return;
        Vector3 posDiff = TargetPoint.position - Camera.transform.position;
        Vector3 angleDiff = TargetPoint.eulerAngles - _StartRotation;

        posDiff.Normalize();
        angleDiff.Normalize();
        factor += Time.deltaTime;
        
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, TargetPoint.position, _Curve.Evaluate(factor));
        Camera.transform.eulerAngles = Vector3.Lerp(Camera.transform.eulerAngles, TargetPoint.eulerAngles, _Curve.Evaluate(factor));

        float posMissing = Vector3.Distance(Camera.transform.position, TargetPoint.position);
        float angleMissing = Quaternion.Angle(Camera.transform.rotation, TargetPoint.rotation);

        if (posMissing < 0.1f) 
            finishedMoving = true;  
        if (Mathf.Abs(angleMissing) < 0.2f) 
            finishedRotating = true;

        if (finishedMoving && finishedRotating)
        {
            opened = false;
            CameraIsMoving = false;
        }
    }
    private void onTouchDown()
    {
        if (CameraIsMoving || opened || ChangeScene.Instance.inTransition)
            return;
        if (!Input.GetMouseButtonDown(0)) 
            return;

        if (allowOnlyExitSign)
        {
            if (!Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, exitSignLayerMask))
                return;


            ChangeScene.Instance.sceneState = ChangeScene.SceneState.WaitForNewScene;
            ExitSignController.Instance.onScreen = _ExitSignOnScreen;
            Camera cameraSnapshot = Camera;
            Vector3 copyPosition = cameraSnapshot.transform.position;
            Vector3 copyRotation = cameraSnapshot.transform.eulerAngles;
            _StartPosition = copyPosition;
            _StartRotation = copyRotation;

            factor = 0f;
            finishedMoving = false;
            finishedRotating = false;
            opened = true;
            CameraIsMoving = true;
            if (_Collider) _Collider.enabled = _ColliderEnabled;
            if (_sceneState != ChangeScene.SceneState.Undefined)
            {
                ChangeScene.Instance.sceneState = _sceneState;
                ChangeScene.Instance.inTransition = true;
            }
            allowOnlyExitSign = false;
        }

        if (!Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity))
            return;

        if (hit.transform == transform)
        {
            ExitSignController.Instance.onScreen = _ExitSignOnScreen;
            Camera cameraSnapshot = Camera;
            Vector3 copyPosition = cameraSnapshot.transform.position;
            Vector3 copyRotation = cameraSnapshot.transform.eulerAngles;
            _StartPosition = copyPosition;
            _StartRotation = copyRotation;

            factor = 0f;
            finishedMoving = false;
            finishedRotating = false;
            opened = true;
            CameraIsMoving = true;
            if (_Collider) _Collider.enabled = _ColliderEnabled;
            if (_sceneState != ChangeScene.SceneState.Undefined)
            {
                ChangeScene.Instance.sceneState = _sceneState;
                ChangeScene.Instance.inTransition = true;
            }
            allowOnlyExitSign = _OnlyExitSign;
        }
    }
    private void checkExit()
    {
        if (!opened && CameraIsMoving && _Collider)
           _Collider.enabled = true;
    }
    public void ForceMove()
    {
        ChangeScene.Instance.sceneState = ChangeScene.SceneState.WaitForNewScene;
        ExitSignController.Instance.onScreen = _ExitSignOnScreen;
        Camera cameraSnapshot = Camera;
        Vector3 copyPosition = cameraSnapshot.transform.position;
        Vector3 copyRotation = cameraSnapshot.transform.eulerAngles;
        _StartPosition = copyPosition;
        _StartRotation = copyRotation;

        factor = 0f;
        finishedMoving = false;
        finishedRotating = false;
        opened = true;
        CameraIsMoving = true;
        if (_Collider) _Collider.enabled = _ColliderEnabled;
        if (_sceneState != ChangeScene.SceneState.Undefined)
        {
            ChangeScene.Instance.sceneState = _sceneState;
            ChangeScene.Instance.inTransition = true;
        }
    }
}
