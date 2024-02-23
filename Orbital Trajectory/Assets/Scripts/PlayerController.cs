using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //the size of the input zone that the player can start any touch input
    public float touchStartRadius;

    public FuelBar fuelBar;
    public ParticleSystem boostParticles;
    public ParticleSystem launchParticles;

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

    [SerializeField]
    private float secondsToPredict;
    [SerializeField]
    private int pointsToDisplay;
    [SerializeField]
    private GameObject indicatorPrefab;
    private List<GameObject> trajectoryIndicators;

    public bool Launched
    {
        get { return launched; }
    }

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

        trajectoryIndicators = new List<GameObject>();
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
                    DestroyIndicators();
                    launched = true;
                    mouseDown = false;
                    rb.velocity = launchSpeed * dragAngle;
                    launchParticles.Play();
                }
            }
            else if (mouseDown)
            {
                dragAngle = -(transform.position - mousePosition).normalized;
                DrawTrajectory();
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
                boostParticles.Play();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mouseDown = false;
                boostParticles.Stop();
            }
            else if (mouseDown)
            {
                float newFuel = fuelBar.slider.value - Time.deltaTime;
                fuelBar.SetFuel(newFuel);
                if(newFuel > 0)
                {
                    rb.AddForce(rb.velocity.normalized * boostForce * Time.deltaTime);
                }
                else
                {
                    boostParticles.Stop();
                }
            }


            //angle the ship towards the velocity
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void DrawTrajectory()
    {
        if(pointsToDisplay > trajectoryIndicators.Count)
        {
            int indicatorsToCreate = pointsToDisplay - trajectoryIndicators.Count;
            for(int i = 0; i < indicatorsToCreate; i++)
            {
                trajectoryIndicators.Add(Instantiate(indicatorPrefab));
            }
        }
        rb.velocity = dragAngle * launchSpeed;
        List<Vector3> points = GravityManager.singleton.CalculateTrajectory(gameObject, pointsToDisplay, secondsToPredict);
        rb.velocity = Vector2.zero;

        for(int i = 0; i < pointsToDisplay; i++)
        {
            trajectoryIndicators[i].SetActive(true);
            trajectoryIndicators[i].transform.position = (Vector2)points[i];
            trajectoryIndicators[i].transform.rotation = Quaternion.Euler(0, 0, points[i].z);
        }

        for(int i = pointsToDisplay; i < trajectoryIndicators.Count; i++)
        {
            trajectoryIndicators[i].SetActive(false);
        }
    }

    private void DestroyIndicators()
    {
        for(int i = 0; i < trajectoryIndicators.Count; i++)
        {
            Destroy(trajectoryIndicators[i]);
        }

        trajectoryIndicators.Clear();
    }

    private void UpdateRadius(float totalTime)
    {
        Color color = radius.GetComponent<SpriteRenderer>().color;
        color.a = Math.Abs((float)Math.Sin(totalTime)/2) - .1f;
        radius.GetComponent<SpriteRenderer>().color = color;
    }

    private void DeactivateRadius()
    {
        Color color = radius.GetComponent<SpriteRenderer>().color;
        color.a -=Time.deltaTime;
        radius.GetComponent<SpriteRenderer>().color = color;
    }
}
