using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager singleton;

    [SerializeField]
    public int gravitationalConstant;
    [SerializeField]
    public List<GravityObject> staticGravityObjects;
    [SerializeField]
    public List<GravityObject> gravityObjects;

    [SerializeField]
    public GameObject trajectoryIndicatorPrefab;

    [SerializeField]
    public bool run;

    public void Start()
    {
        gravityObjects = new List<GravityObject>();
        staticGravityObjects = new List<GravityObject>();
        if(singleton == null)
        {
            singleton = this;
        }
        else
        {
            Debug.Log("Multiple Gravity Managers");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < gravityObjects.Count; i++)
        {
            for(int j = 0; j < staticGravityObjects.Count; j++)
            {
                Vector2 force = CalculateGravity(gravityObjects[i].transform.position, gravityObjects[i].mass, staticGravityObjects[j].transform.position, staticGravityObjects[j].mass, Time.deltaTime);
                gravityObjects[i].rb.AddForce(force);
            }

            for(int j = i + 1; j < gravityObjects.Count; j++)
            {
                Vector2 force = CalculateGravity(gravityObjects[i].transform.position, gravityObjects[i].mass, gravityObjects[j].transform.position, gravityObjects[j].mass, Time.deltaTime);
                gravityObjects[i].rb.AddForce(force);
                gravityObjects[j].rb.AddForce(-force);
            }
        }
    }

    public Vector2 CalculateGravity(Vector2 pos1, float mass1, Vector2 pos2, float mass2, float timeStep)
    {
        Vector2 direction = pos2 - pos1;
        float distance = direction.magnitude;
        float forceMagnitude = gravitationalConstant * (mass1 * mass2)/(distance * distance) * timeStep;

        Vector2 force = direction/distance * forceMagnitude;
        return force;
    }

    public List<Vector3> CalculateTrajectory(GameObject focus, int stepsToDisplay, float time)
    {
        int simSteps = Mathf.CeilToInt(time/Time.fixedDeltaTime);
        if(stepsToDisplay > simSteps)
        {
            Debug.LogError("More points requested than simulation steps in requested time (" + simSteps + " steps will be simulated in " + time + " seconds");
            return null;
        }
        List<Vector2> dynamicObjectPositions = new List<Vector2>();
        List<Vector2> dynamicObjectVelocities = new List<Vector2>();
        List<Vector2> dynamicObjectAccelerations = new List<Vector2>();
        List<Vector3> trajectoryPoints = new List<Vector3>();

        int focusIndex = -1;

        for(int i = 0; i < gravityObjects.Count; i++)
        {
            Rigidbody2D rb = gravityObjects[i].GetComponent<Rigidbody2D>();

            dynamicObjectPositions.Add(gravityObjects[i].transform.position);
            dynamicObjectVelocities.Add(rb.velocity);
            dynamicObjectAccelerations.Add(Vector2.zero);

            if(focus == gravityObjects[i].gameObject)
            {
                focusIndex = i;
            }
        }

        if(focusIndex == -1)
        {
            Debug.Log("Cannot calculate trajectory of " + focus.name + " it is either not a gravity object or it is static.");
            return null;
        }

        int amountBetweenDisplays = simSteps/stepsToDisplay;
        for(int s = 0; s < simSteps; s++)
        {
            for(int i = 0; i < gravityObjects.Count; i++)
            {
                dynamicObjectAccelerations[i] = Vector2.zero;
            }

            for(int i = 0; i < gravityObjects.Count; i++)
            {
                for(int j = 0; j < staticGravityObjects.Count; j++)
                {
                    Vector2 force = CalculateGravity(dynamicObjectPositions[i], gravityObjects[i].mass, staticGravityObjects[j].transform.position, staticGravityObjects[j].mass, Time.fixedDeltaTime);
                    dynamicObjectAccelerations[i] += force/gravityObjects[i].mass;
                }

                for(int j = i + 1; j < gravityObjects.Count; j++)
                {
                    Vector2 force = CalculateGravity(dynamicObjectPositions[i], gravityObjects[i].mass, dynamicObjectPositions[j], gravityObjects[j].mass, Time.fixedDeltaTime);
                    dynamicObjectAccelerations[i] += force/gravityObjects[i].mass;
                    dynamicObjectAccelerations[j] -= force/gravityObjects[j].mass;
                }

                dynamicObjectPositions[i] += (dynamicObjectVelocities[i] * Time.fixedDeltaTime) + (Time.fixedDeltaTime * Time.fixedDeltaTime * dynamicObjectAccelerations[i]);
                dynamicObjectVelocities[i] += dynamicObjectAccelerations[i] * Time.fixedDeltaTime;
            }

            if((s + 1) % amountBetweenDisplays == 0)
            {
                Vector2 position = dynamicObjectPositions[focusIndex];
                Vector2 velocity = dynamicObjectVelocities[focusIndex];
                float angle = MathF.Atan2(velocity.y, velocity.x) * (180.0f/MathF.PI) + 30;
                trajectoryPoints.Add((Vector3)position + (Vector3.forward * angle));

            }
        }

        return trajectoryPoints;
    }


    public void AddGravityObject(GravityObject obj)
    {
        gravityObjects.Add(obj);
    }

    public void AddStaticGravityObject(GravityObject obj)
    {
        staticGravityObjects.Add(obj);
    }
}
