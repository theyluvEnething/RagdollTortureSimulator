using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    [SerializeField][Range(1f, 15f)] private float outlineWidth = 8f;
    [SerializeField] private Color blueOutline = Color.cyan;
    [SerializeField] private Color orangeOutline = new Color(1f, 0.533f, 0f, 255);
    [SerializeField][HideInInspector] private List<GameObject> outlinedObjects;

    private bool once = false;

    void Start()
    {
        GameObject[] blueOutlineObjects = GameObject.FindGameObjectsWithTag("BlueOutline");
        GameObject[] orangeOutlineObjects = GameObject.FindGameObjectsWithTag("OrangeOutline");

        foreach (GameObject gameObject in blueOutlineObjects) outlinedObjects.Add(gameObject);    
        foreach (GameObject gameObject in orangeOutlineObjects) outlinedObjects.Add(gameObject);    
    }
    void Update()
    {
        if (once) return;

        foreach (GameObject obj in outlinedObjects) 
        {
            Outline outline = obj.AddComponent<Outline>();
            outline.enabled = true;
            outline.OutlineWidth = outlineWidth;

            if (obj.CompareTag("BlueOutline"))
            {
                outline.OutlineColor = blueOutline;
            }
            else if (obj.CompareTag("OrangeOutline"))
            {
                outline.OutlineColor = orangeOutline;
            }
        }
        
        once = true;
    }
}
