using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrl : MonoBehaviour
{

    [SerializeField] private string URL;
    
    public void OpenURL()
    {
        Application.OpenURL(URL);
    }
}
