using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ExitSignController : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform OffScreenPoint;
    [SerializeField] private Transform OnScreenPoint;
    [Header("SETTINGS")]
    [SerializeField] private AnimationCurve _Curve;
    [SerializeField] public Boolean onScreen = false;
    [HideInInspector] private bool _onScreen;
    [HideInInspector] private float factor;


    public static ExitSignController Instance;

    void Start()
    {
        if (!Instance)
            Instance = this;
    }
    void FixedUpdate()
    {
        if (onScreen)
            enterScreen();
        else
            leaveScreen();

        if (onScreen != _onScreen)
        {
            factor = 0f;
            _onScreen = onScreen;
        }
    }
    private void enterScreen()
    {
        factor += Time.deltaTime;
        float posMissing = Vector3.Distance(transform.position, OnScreenPoint.position);
        if (posMissing < 0.01f)
        {
            transform.position = OnScreenPoint.position;
            return;

        }
        transform.position = Vector3.Lerp(transform.position, OnScreenPoint.position, _Curve.Evaluate(factor));
     }
    private void leaveScreen()
    {
        factor += Time.deltaTime;
        float posMissing = Vector3.Distance(transform.position, OffScreenPoint.position);
        if (posMissing < 0.01f)
        {
            transform.position = OffScreenPoint.position;
            return;

        }
        transform.position = Vector3.Lerp(transform.position, OffScreenPoint.position, _Curve.Evaluate(factor));
    }
}
