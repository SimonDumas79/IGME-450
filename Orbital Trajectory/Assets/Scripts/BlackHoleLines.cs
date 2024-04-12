using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleLines : MonoBehaviour
{
    float mass;
    float scale;
    ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        mass = GetComponentInParent<GravityObject>().mass;

        //max mass 1500 min 500
        float minMass = 1500;
        float maxMass = 2000;
        float massRatio = (mass - minMass) / (maxMass - minMass);

        float minEmission = 90;
        float maxEmission = 250;

        float minSpeed = -1;
        float maxSpeed = -10;

        float minScale = 3f;
        float maxScale = 25f;

        float minMultiplier = .25f;
        float maxMultiplier = 4.0f;

        float minLifetime = .2f;
        float maxLifetime = 1.0f;

        //set the scale to fit the size of the planet
        ParticleSystem.ShapeModule shapeModule = ps.shape;
        scale = GetComponentInParent<Transform>().lossyScale.x;
        shapeModule.radius = scale * 5;

        //set the emission to fit the mass of the planet
        ParticleSystem.EmissionModule emissionModule = ps.emission;
        emissionModule.rateOverTime = Mathf.Lerp(minEmission, maxEmission, massRatio)
            * Mathf.Lerp(minMultiplier, maxMultiplier, Mathf.Clamp01((scale - minScale) / (maxScale - minScale)));

        //set the speed to fit the mass of the planet
        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.startLifetime = Mathf.Lerp(maxLifetime, minLifetime, massRatio);
        mainModule.startSpeed = Mathf.Lerp(minSpeed, maxSpeed, massRatio);

    }

}
