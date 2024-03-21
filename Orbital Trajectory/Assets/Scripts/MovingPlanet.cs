using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlanet : MonoBehaviour
{
    //the object being orbited around
    public Transform centralBody;

    //Direction to offset the orbiting body by the radius
    public Vector2 startDirection;

    //amount of time taken to complete one full orbit
    public float baseTimeForCycle;

    //rotation in radians around the central body scaled by baseTimeForCycle
    private float rotation;

    //offset from central body to orbit
    public float radius;

    // Start is called before the first frame update
    void Awake()
    {
        startDirection = startDirection.normalized;
        
        rotation = Mathf.Deg2Rad * Vector3.Angle(Vector3.right, startDirection);

        if(startDirection.y < 0)
        {
            rotation = Mathf.PI + (Mathf.PI - rotation);
        }
        Debug.Log(rotation);
    }

    // Update is called once per frame
    void Update()
    {
        rotation += 2 * Mathf.PI * (Time.deltaTime / baseTimeForCycle);

        transform.position = centralBody.transform.position + new Vector3(radius * Mathf.Cos(rotation), radius * Mathf.Sin(rotation), 0);
    }
}
