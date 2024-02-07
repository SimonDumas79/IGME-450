using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //the size of the input zone that the player can start any touch input
    public float touchStartRadius;
    
    //This should be reset every time the player touches within the input radius.
    //drag input's orgin
    private Vector2 touchStartPoint;

    //angle between current startPosition and the current touchPosition, controls angle of flight
    private Vector2 dragAngle;

    private bool mouseDown;

    public GameObject radius;

    float currentWidth;  // current width
    float scaleFactor;

    void Start()
    {
        Input.simulateMouseWithTouches = true;
        dragAngle = new Vector2();
        currentWidth = radius.transform.localScale.x;
        scaleFactor = touchStartRadius / currentWidth;
        radius.transform.localScale = new Vector3(scaleFactor/2.5f, scaleFactor/2.5f, radius.transform.localScale.z);
    }

    void Update()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0)) 
        {
            float squaredDistance = (transform.position.x - mousePosition.x) * (transform.position.x - mousePosition.x) + (transform.position.y - mousePosition.y) * (transform.position.y - mousePosition.y);

            if (squaredDistance < touchStartRadius * touchStartRadius)
            {
                touchStartPoint = Input.mousePosition;
                mouseDown = true;

                //Debug.Log($"{Input.mousePosition}, {transform.position}, {squaredDistance}");
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
        }
        else if (mouseDown)
        {
            dragAngle = (transform.position - mousePosition).normalized;
            Debug.Log(dragAngle);
        }

        //angle the ship towards the shot angle
        float angle = Mathf.Atan2(dragAngle.y, dragAngle.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        UpdateRadius(Time.realtimeSinceStartup);
    }

    private void UpdateRadius(float totalTime)
    {
        Color color = radius.GetComponent<SpriteRenderer>().color;
        color.a = Math.Abs((float)(Math.Sin(totalTime/2))/2)-.25f;
        radius.GetComponent<SpriteRenderer>().color = color;
    }
}
