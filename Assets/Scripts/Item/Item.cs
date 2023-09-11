using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour
{
    [Header("Player")]
    [HideInInspector] public GameObject _Player;
    [HideInInspector] public Transform _PlayerTransform;
    [HideInInspector] public Character _Character;
    [HideInInspector] public RagdollEnabler _RagdollEnabler;
    [HideInInspector] public bool isDragged;
    [HideInInspector] public Vector2 _CrosshairOffset;

    void Start()
    {
        _Player = DataManager.Instance.Player;
        _PlayerTransform = DataManager.Instance.PlayerTransform;
        _Character = DataManager.Instance.Character;
        _RagdollEnabler = DataManager.Instance.RagdollEnabler;
        _CrosshairOffset = new Vector2(70, -45);
    }
}
