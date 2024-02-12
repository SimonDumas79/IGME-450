using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityObject : MonoBehaviour
{
    [SerializeField]
    public float mass = 1;
    [SerializeField]
    private Vector2 initialVelocity;
    [SerializeField]
    private bool isKinematic = true;
    public Rigidbody2D rb;
    //public float radius = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;
        rb.velocity = initialVelocity;

        if(isKinematic)
        {
            GravityManager.singleton.AddGravityObject(this);
        }
        else
        {
            GravityManager.singleton.AddStaticGravityObject(this);
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rb.transform.position, radius);
    }*/
}
