using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextCash : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private TextMeshProUGUI cashCounter;
    [SerializeField] private ParticleSystem cashAnimation;
    [SerializeField] private RectTransform cashPopUp;
    [SerializeField] private Animator _animator;

    private int oldCash;

    void Start()
    {
        if (!_animator) 
            _animator = GetComponent<Animator>();
        oldCash = 0;
    }

    void Update()
    {
        _animator.SetBool("PlayCashAnimation", false);
        if (oldCash < GameManager.Instance.GetCash())
        {
            _animator.SetBool("PlayCashAnimation", true);
            //StartCoroutine(PopUp(GameManager.Instance.cash - oldCash));
        }
        cashCounter.text = GameManager.Instance.GetCash().ToString();
        oldCash = GameManager.Instance.GetCash();
    }

    private IEnumerator PopUp(int difference)
    {
        RectTransform cashObj = Instantiate(cashPopUp, this.transform);
        TextMeshProUGUI cashText = cashObj.GetComponent<TextMeshProUGUI>();
        cashText.text = difference.ToString();

        yield return null;
    }
}
