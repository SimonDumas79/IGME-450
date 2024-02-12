using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //the size of the input zone that the player can start any touch input
    public float touchStartRadius;

    public FuelBar fuelBar;

    public float launchSpeed;

    private Vector2 startPosition;

    //This should be reset every time the player touches within the input radius.
    //drag input's orgin
    private Vector2 touchStartPoint;

    //angle between current startPosition and the current touchPosition, controls angle of flight
    private Vector2 dragAngle;

    private bool mouseDown;

    public GameObject radius;

    float currentWidth;  // current width
    float scaleFactor;

    bool launched = false;

    public float boostTime = 2.0f;
    public float boostForce = 2.0f;
    private Rigidbody2D rb;
    

    void Start()
    {
        Input.simulateMouseWithTouches = true;
        dragAngle = new Vector2(0,1);
        currentWidth = radius.transform.localScale.x;
        scaleFactor = touchStartRadius / currentWidth;
        radius.transform.localScale = new Vector3(scaleFactor/2.5f, scaleFactor/2.5f, radius.transform.localScale.z);

        rb = GetComponent<Rigidbody2D>();
        fuelBar.SetMaxFuel(boostTime);
        startPosition = transform.position;
    }

    void Update()
    {
        if(!launched)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(Input.GetMouseButtonDown(0)) 
            {
                float squaredDistance = (transform.position.x - mousePosition.x) * (transform.position.x - mousePosition.x) + (transform.position.y - mousePosition.y) * (transform.position.y - mousePosition.y);

                if (squaredDistance < touchStartRadius * touchStartRadius)
                {
                    touchStartPoint = Input.mousePosition;
                    mouseDown = true;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if(mouseDown)
                {
                    launched = true;
                    mouseDown = false;
                    rb.velocity = launchSpeed * dragAngle;
                }
            }
            else if (mouseDown)
            {
                dragAngle = -(transform.position - mousePosition).normalized;
                Debug.Log(dragAngle);
            }

            //angle the ship towards the shot angle
            float angle = Mathf.Atan2(dragAngle.y, dragAngle.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            UpdateRadius(Time.realtimeSinceStartup);
            transform.position = startPosition;
        }
        else
        {
            DeactivateRadius();
            if (Input.GetMouseButtonDown(0))
            {
                mouseDown = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mouseDown = false;
            }
            else if (mouseDown)
            {
                float newFuel = fuelBar.slider.value - Time.deltaTime; 
                fuelBar.SetFuel(newFuel);

                rb.AddForce(rb.velocity.normalized * boostForce * Time.deltaTime);
            }


            //angle the ship towards the velocity
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void UpdateRadius(float totalTime)
    {
        Color color = radius.GetComponent<SpriteRenderer>().color;
        color.a = Math.Abs((float)(Math.Sin(totalTime))/2) - .1f;
        radius.GetComponent<SpriteRenderer>().color = color;
    }

    private void DeactivateRadius()
    {
        Color color = radius.GetComponent<SpriteRenderer>().color;
        color.a -=Time.deltaTime;
        radius.GetComponent<SpriteRenderer>().color = color;
    }
}
