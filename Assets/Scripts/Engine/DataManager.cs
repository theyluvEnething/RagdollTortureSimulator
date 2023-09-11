using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    [HideInInspector] public static DataManager Instance;

    [SerializeField] public GameObject Player;
    [SerializeField] public Transform PlayerTransform;
    [SerializeField] public Character Character;
    [SerializeField] public RagdollEnabler RagdollEnabler;
    [SerializeField] public RagdollAnimator RagdollAnimator;
    [SerializeField] public Item Item;
    [SerializeField] public Camera MainCamera;
    [SerializeField] public ResetLevel ResetLevel;
    [SerializeField] public Healthbar Healthbar;
    [SerializeField] public Transform ItemGroup;
    [SerializeField] public InterstitialAdsButton AdsManager;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
}
