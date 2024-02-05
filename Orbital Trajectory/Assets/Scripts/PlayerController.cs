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

    void Start()
    {
        Input.simulateMouseWithTouches = true;
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(0)) 
        {
            
            float squaredDistance = (transform.position.x - Input.mousePosition.x) * (transform.position.x - Input.mousePosition.x) + (transform.position.y - Input.mousePosition.y) * (transform.position.y - Input.mousePosition.y);

            if (squaredDistance < touchStartRadius * touchStartRadius)
            {
                touchStartPoint = Input.mousePosition;
                mouseDown = true;
            }
        }

        if (mouseDown)
        {
            dragAngle = (touchStartPoint - new Vector2(Input.mousePosition.x, Input.mousePosition.y)).normalized;
        }

        if (Input.GetMouseButtonUp(0))
        {

            mouseDown = false;
        }


        //get and handle touch and drag input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //if a touch just started, check if it is in the touch radius and store the position if it is
            if(touch.phase == TouchPhase.Began ) 
            {
                float squaredDistance = (transform.position.x - touch.position.x) * (transform.position.x - touch.position.x) + (transform.position.y - touch.position.y) * (transform.position.y - touch.position.y);

                if(squaredDistance < touchStartRadius * touchStartRadius)
                {
                    touchStartPoint = touch.position;
                }

            }
            //change the angle of the ship based on touch drag
            else if(touch.phase == TouchPhase.Moved )
            {
                dragAngle = (touchStartPoint - touch.position).normalized;
            }
            
            else if( touch.phase == TouchPhase.Ended ) 
            {

            }
            Debug.Log(dragAngle);
        }
    }
}
