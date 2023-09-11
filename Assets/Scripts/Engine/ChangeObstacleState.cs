using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObstacleState : MonoBehaviour
{
    [SerializeField] private List<GameObject> backgroundObjects = new List<GameObject>();
    [SerializeField] public bool activateObstacles = true;
    private bool isActiveObstacles;

    public static ChangeObstacleState Instace;

    void Start()
    {
        if (!Instace)
        {
            Instace = this;
        }
        isActiveObstacles = activateObstacles;
    }
    void Update()
    {
        if (activateObstacles && !isActiveObstacles)
        {
            ActivateObstacles(backgroundObjects);
            isActiveObstacles = true;
        }
        if (!activateObstacles && isActiveObstacles)
        {
            DeactivateObstacles(backgroundObjects);
            isActiveObstacles = false;
        }
    }
    private void ActivateObstacles(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
            obj.SetActive(true);
    }
    private void DeactivateObstacles(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
            obj.SetActive(false);
    }
}
