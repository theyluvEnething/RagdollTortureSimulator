using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextGold : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private TextMeshProUGUI goldCounter;
    [SerializeField] private ParticleSystem goldAnimation;
    [SerializeField] private RectTransform goldPopUp;
    [SerializeField] private Animator _animator;

    private int oldGold;
    void Start()
    {
        if (!_animator)
            _animator = GetComponent<Animator>();
        oldGold = GameManager.Instance.GetGold();
    }

    void Update()
    {
        _animator.SetBool("PlayGoldAnimation", false);
        if (oldGold < GameManager.Instance.GetGold())
        {
            _animator.SetBool("PlayGoldAnimation", true);
            //StartCoroutine(PopUp(GameManager.Instance.cash - oldCash));
        }
        goldCounter.text = GameManager.Instance.GetGold().ToString();
        oldGold = GameManager.Instance.GetGold();
    }

    private IEnumerator PopUp(int difference)
    {
        RectTransform cashObj = Instantiate(goldPopUp, this.transform);
        TextMeshProUGUI cashText = cashObj.GetComponent<TextMeshProUGUI>();
        cashText.text = difference.ToString();

        yield return null;
    }
}
