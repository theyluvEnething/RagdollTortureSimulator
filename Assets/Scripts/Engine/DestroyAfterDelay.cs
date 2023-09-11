using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField] private float Delay = 2f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(Delay);
        Destroy(gameObject);
    }
}
