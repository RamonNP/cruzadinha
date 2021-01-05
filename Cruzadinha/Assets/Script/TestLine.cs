using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLine : MonoBehaviour
{
  private LineRenderer _lineRenderer;
     public void Start()
     {
         _lineRenderer = GetComponent<LineRenderer>();
         _lineRenderer.SetWidth(0.2f, 0.2f);
         _lineRenderer.enabled = false;
     }
 
     public Vector3 _initialPosition;
     public Vector3 _currentPosition;
     public Vector3 testeT;
     public Vector3 testeM;
     public void Update()
     {
         Touch touch = simulatess();
         if (touch.phase == TouchPhase.Began)
         {
             _initialPosition = GetCurrentMousePosition(touch.position).GetValueOrDefault();
             _lineRenderer.SetPosition(0, _initialPosition);
             _lineRenderer.SetVertexCount(1);
             _lineRenderer.enabled = true;
         } 
         else if ((touch.phase == TouchPhase.Moved))
         {
             _currentPosition = GetCurrentMousePosition(touch.position).GetValueOrDefault();
             _lineRenderer.SetVertexCount(2);
             _lineRenderer.SetPosition(1, _currentPosition);
 
         } 
         else if ((touch.phase == TouchPhase.Ended))
         {
             _lineRenderer.enabled = false;
             var releasePosition = GetCurrentMousePosition(touch.position).GetValueOrDefault();
             var direction = releasePosition - _initialPosition;
             Debug.Log("Process direction " + direction);
         }
     }
 
     private Vector3? GetCurrentMousePosition(Vector3 pos)
     {
         var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         var plane = new Plane(Vector3.forward, Vector3.zero);
 
         float rayDistance;
         if (plane.Raycast(ray, out rayDistance))
         {
             return ray.GetPoint(rayDistance);
             
         }
 
         return null;
     }

     private Touch simulatess()
    {
        Touch touch = new Touch();
        if (Input.GetMouseButtonDown(0))
        {
            touch = new Touch();
            touch.phase = TouchPhase.Began;
            touch.position = Input.mousePosition;
        } else if (Input.GetMouseButton(0))
        {
            touch = new Touch();
            touch.phase = TouchPhase.Moved;
            touch.position = Input.mousePosition;
            testeT = touch.position;
            testeM = Input.mousePosition;
        } else if (Input.GetMouseButtonUp(0))
        {
            touch = new Touch();
            touch.phase = TouchPhase.Ended;
            touch.position = Input.mousePosition;
        } else {
            touch = new Touch();
            touch.phase = TouchPhase.Canceled;
        }
        return touch;
    }
}
