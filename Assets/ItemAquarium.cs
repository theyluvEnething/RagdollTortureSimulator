using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using static ChangeScene;
using System.Numerics;
using System.Security.Cryptography;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class ItemAquarium : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform _Ground;
    [SerializeField] private float test;

    [Header("REFERENCES")]
    [SerializeField] private Transform TargetPoint;
    [SerializeField] private Vector3 _StartPosition;
    [SerializeField] private Vector3 _StartRotation;

    [Header("SETTINGS")]
    [SerializeField] private AnimationCurve _Curve;
    [SerializeField] private bool _ExitSignOnScreen = false;
    [SerializeField] private bool _ColliderEnabled = false;
    [SerializeField] private ChangeScene.SceneState _sceneState;

    [SerializeField] public static bool CameraIsMoving = false;
    [SerializeField] public static bool allowOnlyExitSign = false;

    private bool opened, finishedRotating, finishedMoving;
    private Collider _Collider;
    private Camera Camera;
    private float factor;
    private int exitSignLayerMask;
    private Vector3 inputPosition;

    void Start()
    {
        Camera = DataManager.Instance.MainCamera;
    }
    void Update()
    {
        if (opened)
        {
            SmoothFollow();
            return;
        }

        if (!Input.GetMouseButtonDown(0))
            return;

        inputPosition = Input.mousePosition;
        if (!Physics.Raycast(Camera.ScreenPointToRay(inputPosition), out RaycastHit hit, Mathf.Infinity))
            return;

        if (hit.transform == transform)
        {
            DataManager.Instance.RagdollAnimator.PushRagdoll();
            Tween.LocalScale(transform, 0.01f, 0.5f, Ease.OutSine);
            Tween.PositionY(transform, 5f, 0.5f, Ease.OutSine);
            Tween.PositionZ(_Ground, 15f, 0.5f, Ease.OutSine);

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
}
