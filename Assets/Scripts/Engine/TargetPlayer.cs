using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    public GameObject player;
    private Transform playerTransform;
    [Range(0f, 10f)]
    public float flightSpeed;

    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 direction = playerTransform.position - this.transform.position;
        transform.position = direction * flightSpeed;
    }
}
