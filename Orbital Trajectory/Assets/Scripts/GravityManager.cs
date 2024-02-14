using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager singleton;

    [SerializeField]
    private int gravitationalConstant;
    [SerializeField]
    private List<GravityObject> staticGravityObjects;
    [SerializeField]
    private List<GravityObject> gravityObjects;

    [SerializeField]
    private int trajectorySteps;
    [SerializeField]
    private float trajectoryTime;
    [SerializeField]
    private GameObject focusObj;
    [SerializeField]
    private GameObject trajectoryIndicator;
    private List<GameObject> trajectoryObjects;

    [SerializeField]
    public bool run;

    void Start()
    {
        trajectoryObjects = new List<GameObject>();
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
        if(run)
        {
            Time.timeScale = 1;
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
        else
        {
            Time.timeScale = 0;
            List<Vector2> traj = CalculateTrajectory(focusObj, trajectorySteps, trajectoryTime);
            for(int i = 0; i < trajectorySteps; i++)
            {
                if(trajectoryObjects.Count > i)
                {
                    trajectoryObjects[i].transform.position = traj[i];
                }
                else
                {
                    trajectoryObjects.Add(Instantiate(trajectoryIndicator, traj[i], Quaternion.identity));
                }
            }
        }
    }



    private Vector2 CalculateGravity(Vector2 pos1, float mass1, Vector2 pos2, float mass2, float timeStep)
    {
        Vector2 direction = pos2 - pos1;
        float distance = direction.magnitude;
        float forceMagnitude = gravitationalConstant * (mass1 * mass2)/(distance * distance) * timeStep;

        Vector2 force = (direction/distance) * forceMagnitude;
        return force;
    }

    private List<Vector2> CalculateTrajectory(GameObject focus, int steps, float time)
    {
        List<Vector2> dynamicObjectPositions = new List<Vector2>();
        List<Vector2> dynamicObjectVelocities = new List<Vector2>();
        List<Vector2> dynamicObjectAccelerations = new List<Vector2>();
        List<Vector2> trajectoryPoints = new List<Vector2>();
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

        float timeSteps = time/steps;
        for(int s = 0; s < steps; s++)
        {

            for(int i = 0; i < gravityObjects.Count; i++)
            {
                dynamicObjectAccelerations[i] = Vector2.zero;
            }

            for(int i = 0; i < gravityObjects.Count; i++)
            {
                for(int j = 0; j < staticGravityObjects.Count; j++)
                {
                    Vector2 force = CalculateGravity(dynamicObjectPositions[i], gravityObjects[i].mass, staticGravityObjects[j].transform.position, staticGravityObjects[j].mass, timeSteps);
                    dynamicObjectAccelerations[i] += force/gravityObjects[i].mass;
                }

                for(int j = i + 1; j < gravityObjects.Count; j++)
                {
                    Vector2 force = CalculateGravity(dynamicObjectPositions[i], gravityObjects[i].mass, dynamicObjectPositions[i], gravityObjects[j].mass, timeSteps);
                    dynamicObjectAccelerations[i] += force/gravityObjects[i].mass;
                    dynamicObjectAccelerations[j] -= force/gravityObjects[j].mass;
                }

                dynamicObjectPositions[i] += (dynamicObjectVelocities[i] * timeSteps) + (timeSteps * timeSteps * dynamicObjectAccelerations[i]);
                dynamicObjectVelocities[i] += dynamicObjectAccelerations[i] * timeSteps;
            }

            trajectoryPoints.Add(dynamicObjectPositions[focusIndex]);
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
