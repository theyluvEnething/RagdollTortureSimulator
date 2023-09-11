using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class GoldAnimationController : MonoBehaviour
{
    void Start()
    {
        float scale = 1.25f;
        float duration = 0.2f;
        Tween.LocalScaleY(transform, transform.localScale.y * 5f, duration, Ease.OutSine, 2, CycleMode.Yoyo);
        Tween.LocalScaleX(transform, transform.localScale.x * scale, duration, Ease.OutSine, 2, CycleMode.Yoyo);
        Tween.LocalScaleZ(transform, transform.localScale.z * scale, duration, Ease.OutSine, 2, CycleMode.Yoyo);
        Tween.LocalPositionX(transform, transform.localPosition.x * scale, duration, Ease.OutSine, 2, CycleMode.Yoyo);
        Tween.LocalPositionY(transform, transform.localPosition.y * scale * 1.5f, duration, Ease.OutSine, 2, CycleMode.Yoyo);
    }
}
