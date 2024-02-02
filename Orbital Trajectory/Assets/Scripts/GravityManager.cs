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

    void Start()
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
                Vector2 direction = staticGravityObjects[j].transform.position - gravityObjects[i].transform.position;
                float distance = direction.magnitude;
                float forceMagnitude = gravitationalConstant * (gravityObjects[i].mass * gravityObjects[j].mass)/(distance * distance) * Time.deltaTime;

                Vector2 force = (direction/distance) * forceMagnitude;
                gravityObjects[i].rb.AddForce(force);
            }

            for(int j = i + 1; j < gravityObjects.Count; j++)
            {
                Vector2 direction = gravityObjects[j].transform.position - gravityObjects[i].transform.position;
                float distance = direction.magnitude;
                float forceMagnitude = gravitationalConstant * (gravityObjects[i].mass * gravityObjects[j].mass)/(distance * distance) * Time.deltaTime;

                Vector2 force = (direction/distance) * forceMagnitude;
                gravityObjects[i].rb.AddForce(force);
                gravityObjects[j].rb.AddForce(-force);
            }
        }
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
