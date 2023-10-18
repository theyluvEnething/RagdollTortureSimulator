using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWindow : MonoBehaviour
{
    public void Close(GameObject WindowToClose)
    {
        Destroy(WindowToClose);
    }
}
