using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityObject : MonoBehaviour
{
    [SerializeField]
    public float mass = 1;
    [SerializeField]
    public Vector2 initialVelocity;
    [SerializeField]
    private bool isKinematic = true;
    public Rigidbody2D rb;
    private List<GameObject> trajectoryPoints;

    [SerializeField]
    private int pointsToDisplay;
    [SerializeField]
    private float timeToPredict;
    GravityManager gravityManager;

    [SerializeField]
    private GravityObject toOrbit;

    public void Start()
    {
        gravityManager = GravityManager.singleton;

        if(toOrbit != null)
        {
            Vector2 distance = toOrbit.transform.position - transform.position;
            float speed = MathF.Sqrt(gravityManager.gravitationalConstant * toOrbit.mass/(distance.magnitude * 50));
            Vector2 direction = distance.normalized;
            Vector2 orbitDirection = new Vector2(-direction.y, direction.x);
            orbitDirection *= speed;
            initialVelocity = orbitDirection;
        }

        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;
        rb.velocity = initialVelocity;
        rb.bodyType = isKinematic ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;

        if(isKinematic)
        {
            gravityManager.AddGravityObject(this);
        }
        else
        {
            gravityManager.AddStaticGravityObject(this);
        }
    }


    public void DeleteTrajectoryPoints()
    {
        if(trajectoryPoints == null)
        {
            trajectoryPoints = new List<GameObject>();
        }
        for(int i = 0; i < trajectoryPoints.Count; i++)
        {
            if(Application.isPlaying)
            {
                Destroy(trajectoryPoints[i]);
            }
            else
            {
                DestroyImmediate(trajectoryPoints[i]);
            }
        }

        trajectoryPoints.Clear();
    }

    public void DrawTrajectory()
    {
        if(trajectoryPoints == null)
        {
            trajectoryPoints = new List<GameObject>();
        }
        if(pointsToDisplay > trajectoryPoints.Count)
        {
            int indicatorsToCreate = pointsToDisplay - trajectoryPoints.Count;
            for(int i = 0; i < indicatorsToCreate; i++)
            {
                trajectoryPoints.Add(Instantiate(gravityManager.trajectoryIndicatorPrefab));
            }
        }

        List<Vector3> points = GravityManager.singleton.CalculateTrajectory(gameObject, pointsToDisplay, timeToPredict);

        for(int i = 0; i < pointsToDisplay; i++)
        {
            trajectoryPoints[i].SetActive(true);
            trajectoryPoints[i].transform.position = (Vector2)points[i];
            trajectoryPoints[i].transform.rotation = Quaternion.Euler(0, 0, points[i].z);
        }

        for(int i = pointsToDisplay; i < trajectoryPoints.Count; i++)
        {
            trajectoryPoints[i].SetActive(false);
        }
    }
}
