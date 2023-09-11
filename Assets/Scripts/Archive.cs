// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.EventSystems;
// 
// public class MoveObjects : MonoBehaviour
// {
//     private Transform selectedObject;
//     private RaycastHit raycastHit;
//     private bool isPickedUp;
//     private Vector3 mouseOffset;
//     private float mZCoord;
// 
//     void Update()
//     {
// 
// 
//         // If you let go of the player
//         if (Input.GetMouseButtonUp(0)) isPickedUp = false;
// 
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         RaycastHit raycastHit;
//         // 9 == MOVABLE TAG
//         if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit, Mathf.Infinity, 9))
//         {
//             selectedObject = raycastHit.transform;
// 
//             // While holding player
//             if (Input.GetMouseButtonDown(0))
//             {
//                 isPickedUp = true;
//                 mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
//                 mouseOffset = gameObject.transform.position - getMouseWorldPos();
//             }
//             // DEBUG
//             Debug.Log(isPickedUp);
//         }
// 
// 
//         if (isPickedUp)
//         {
//             transform.position = getMouseWorldPos() + mouseOffset;
//         }
//     }
//     /*
//     void OnMouseDown()
//     {
//         if (selectedObject != null)
//         { 
//             mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
//             mouseOffset = gameObject.transform.position - getMouseWorldPos();
//         }
//     }
//     */
// 
// 
// void OnMouseDrag()
// {
//     transform.position = getMouseWorldPos() + mouseOffset;
// }
// 
// 
// using UnityEngine;
// 
// private Vector3 getMouseWorldPos()
// {
//     // Pixel Coordinates 
//     Vector3 mouse = Input.mousePosition;
//     // Z Coordinate of the GameObject
//     mouse.z = mZCoord;
// 
//     return Camera.main.ScreenToWorldPoint(mouse);
// }
// }





// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using Unity.VisualScripting;
// using UnityEngine;
// 
// public class DragObject : MonoBehaviour
// {
//     private Vector3 mouseOffset;
//     private float mZCoord;
//     private Vector3 dragVelocity;
//     private Rigidbody rb;
// 
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }
// 
//     void LateUpdate()
//     {
//         if (rb != null && Input.GetMouseButtonUp(0) && dragVelocity.magnitude >= 5)
//         {
//             rb.velocity = dragVelocity;
//             dragVelocity *= 0.95f;
//             Debug.Log(dragVelocity);
//         }
//     }
// 
//     void OnMouseDown()
//     {
//         mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
//         mouseOffset = gameObject.transform.position - getMouseWorldPos();
// 
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         RaycastHit hit;
// 
//         // Perform the initial raycast
//         if (Physics.Raycast(ray, out hit))
//         {
//             Debug.Log(hit);
//         }
//     }
// 
// 
//     void OnMouseDrag()
//     {
//         Vector3 oldTransform = transform.position;
//         transform.position = getMouseWorldPos() + mouseOffset;
//         dragVelocity = (transform.position - oldTransform) * 200f;
//     }
// 
// 
//     private Vector3 getMouseWorldPos()
//     {
//         // Pixel Coordinates 
//         Vector3 mouse = Input.mousePosition;
//         // Z Coordinate of the GameObject
//         mouse.z = mZCoord;
// 
//         return Camera.main.ScreenToWorldPoint(mouse);
//     }
// }



