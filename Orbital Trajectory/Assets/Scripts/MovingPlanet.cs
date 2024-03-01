using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlanet : MonoBehaviour
{
    //the object being orbited around
    public Transform centralBody;

    public Vector3 startDirection;

    //amount of time taken to complete one full orbit
    public float baseTimeForCycle;
    private float timeForCycle;

    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = centralBody.position + startDirection * radius;
    }

    // Update is called once per frame
    void Update()
    {
        Time.
    }
}
