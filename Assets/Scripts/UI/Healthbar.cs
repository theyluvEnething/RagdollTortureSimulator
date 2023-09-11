using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Healthbar : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _healthbarTransform;
    [SerializeField] private Image _healthbarSprite;

    [Header("SETTINGS")]
    [SerializeField] private float reduceSpeed = 3f;
    [SerializeField] private Gradient _colorGradient;
    private float target = 1f;

    private Camera _camera;
    private float oldFillAmount;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        target = currentHealth / maxHealth;
    }
    void Update()
    {
        Vector3 copyPosition = _headTransform.position;
        copyPosition.y += 2f;
        _healthbarTransform.position = copyPosition;
        _healthbarTransform.rotation = Quaternion.LookRotation(_healthbarTransform.position - _camera.transform.position);

        if (_healthbarSprite.fillAmount - target < 0.5f)
            _healthbarSprite.fillAmount = Mathf.MoveTowards(_healthbarSprite.fillAmount, target, reduceSpeed * Time.deltaTime);
        else
            _healthbarSprite.fillAmount = Mathf.MoveTowards(_healthbarSprite.fillAmount, target, reduceSpeed * 5 * Time.deltaTime);

        _healthbarSprite.color = _colorGradient.Evaluate(1 - _healthbarSprite.fillAmount);
    }
    void Start()
    {
        _camera = DataManager.Instance.MainCamera;
    }
}
