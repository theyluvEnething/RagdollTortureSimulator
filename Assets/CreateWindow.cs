using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWindow : MonoBehaviour
{
    public void Create(GameObject Window)
    {
        Debug.Log("Created Button");
        Instantiate(Window);
    }
}
