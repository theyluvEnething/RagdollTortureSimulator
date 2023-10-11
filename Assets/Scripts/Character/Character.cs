using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private ResetLevel _resetLevel;
    [SerializeField] private Healthbar _healthbar;
    [SerializeField] private ParticleSystem _BloodSplash;
    [SerializeField] private AudioClip[] _AudioClips;
    [SerializeField] private GameObject _GoldText;
    [SerializeField] private Transform ScreenMiddle;

    [Header("ATTRIBUTES")]
    [SerializeField] public int MAX_HEALTH = 20000;
    [SerializeField] private int health = 0;
    [SerializeField] public bool isDead = false;

    private AudioSource _AudioSource;
    private Transform _PlayerTransform;
    private bool resetted;
    private float bloodTimer, soundTimer;
    public static Character Instance { get; private set; }

    void Start()
    {
        if (Instance)
        {
            Debug.LogError("More than one Character Instance in this scene. ");
        }
        Instance = this;
        _AudioSource = GetComponent<AudioSource>();
        MAX_HEALTH += (500 * GameManager.Instance.GetDeathCount());
        health = MAX_HEALTH;
        _resetLevel = DataManager.Instance.ResetLevel;
        _healthbar = DataManager.Instance.Healthbar;
        _PlayerTransform = DataManager.Instance.PlayerTransform;
        _healthbar.UpdateHealthBar(health, MAX_HEALTH);
    }
    void LateUpdate()
    {
        bloodTimer -= Time.deltaTime;
        soundTimer -= Time.deltaTime;

        if (health <= 0 && !isDead)
        {
            isDead = true;
            GameManager.Instance.IncreaseDeathCount();

            float rng = Random.Range(0f, 1f);
            if (rng < 0.15f)
            {
                Vector3 spawnpoint = new Vector3(ScreenMiddle.position.x + _GoldText.transform.localScale.x, ScreenMiddle.position.y, ScreenMiddle.position.z);
                spawnpoint += (_PlayerTransform.position - ScreenMiddle.transform.position).normalized * 5f;
                Instantiate(_GoldText, spawnpoint, Quaternion.identity);
                GameManager.Instance.addGold(50);
            }
        }
        if (isDead && !resetted)
        {
            _resetLevel.resetLevel();
            resetted = true;
        }
    }
    public void Damage(int amount)
    {
        if (GameManager.Instance.GetBloodUnlocked())
            SpawnBlood();

        PlayRandomSound();

        health -= amount;
        _healthbar.UpdateHealthBar(health, MAX_HEALTH);
        int cash =  10 * ((int) Random.Range(1, Mathf.Clamp(Mathf.Round(amount / 50)/2, 1, Mathf.Infinity))); 
        GameManager.Instance.addCash(cash);
    }
    public void Damage(int amount, bool blood)
    {
        if (blood)
            SpawnBlood();

        PlayRandomSound();

        health -= amount;
        _healthbar.UpdateHealthBar(health, MAX_HEALTH);
        int cash = 10 * ((int)Random.Range(1, Mathf.Clamp(Mathf.Round(amount / 50) / 2, 1, Mathf.Infinity)));
        GameManager.Instance.addCash(cash);
    }
    public void Kill()
    {
        Damage(health);
    }
    private void SpawnBlood()
    {
        if (bloodTimer <= 0)
        {
            Instantiate(_BloodSplash, _PlayerTransform);
            bloodTimer = 1f;
        }
    }
    private void PlayRandomSound()
    {
        if (soundTimer > 0)
            return;

        AudioClip clip = _AudioClips[Random.Range(0, _AudioClips.Length)];
        _AudioSource.PlayOneShot(clip);
        soundTimer = clip.length * 0.75f;
    }
}
