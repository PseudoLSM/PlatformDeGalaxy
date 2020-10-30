﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Vector2 RotateVector(Vector2 v, float degrees) // Method for rotating Vector2D's           Do Not Change
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    [SerializeField] protected float xAcceleration;
    [SerializeField] protected float ScanRadius;

    [SerializeField] protected LayerMask LayerToCheck;

    protected Rigidbody2D rb;
    protected Collider2D cldr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cldr = GetComponent<Collider2D>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0) // Checks for horizontal movement input
        {
            Collider2D[] Planets = Physics2D.OverlapCircleAll(transform.position, ScanRadius, LayerToCheck); // Finds planets withing range ```ScanRadius```

            float[] PlanetGravities = new float[Planets.Length]; // Creates array that will be filled with the magnitudes of the gravities from planets within the scan radius

            for (var i = 0; i < Planets.Length; i++)
            {
                PlanetGravities[i] = Planets[i].GetComponent<PlanetGravity>().ForceOfGravity.magnitude; // Populates the empty array with the needed data
                Debug.Log(PlanetGravities[i]); // Checker
            }

            for (var i = 0; i < PlanetGravities.Length; i++) // Checks for the Planet you are on
            {
                if (Mathf.Abs(Planets[i].Distance(cldr).distance) <= 0.1) // Chooses the planet that you are on
                {
                    Vector2 direction = transform.position - Planets[i].transform.position; // Creates vector in planet's direction

                    Vector2 fixedDirection = RotateVector(direction, 270f); // Utilizes earlier method to rotate the vector 270 degrees to get everything aligned

                    rb.AddForce(fixedDirection.normalized * Mathf.Sign(Input.GetAxis("Horizontal")) * xAcceleration * Time.deltaTime); // Adds the force to accelerate the player character
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
