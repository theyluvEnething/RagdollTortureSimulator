using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    /*
    [Header("REFERENCES")]
    [SerializeField] private RectTransform _PrefabCashPopUp;
    [SerializeField] private Transform _CashUIPosition;
    [Header("ATTRIBUTES")]
    [SerializeField] private AnimationCurve _Curve;
    public bool debugBool;
    public void cashAnimation(int amount)
    {
        RectTransform rectTransform = (RectTransform) Instantiate(_PrefabCashPopUp, _CashUIPosition.position, _CashUIPosition.rotation, _CashUIPosition);
        StartCoroutine(PopUpAnimation(rectTransform));
    }
    private IEnumerator PopUpAnimation(RectTransform rectTransform)
    {
        float timer = 0f;
        while (timer < 1f)
        {
            RectTransform snapshot = rectTransform;
            snapshot.position = new Vector3(snapshot.position.x, snapshot.position.y - _Curve.Evaluate(timer * 5f), snapshot.position.z);
            rectTransform = snapshot;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        if (debugBool)
        {
            cashAnimation(50);
            debugBool = false;
        }

    }*/
}
