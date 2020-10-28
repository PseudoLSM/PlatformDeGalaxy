using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Vector2 Rotate(Vector2 v, float degrees) // Method for rotating Vector2D's
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    public float xAcceleration;
    public float ScanRadius;

    public LayerMask LayerToCheck;

    Rigidbody2D rb;
    Collider2D cldr;

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

            for (var i = 0; i < Planets.Length; i++) // Checks for the Planet you are on
            {
                if (Mathf.Abs(Planets[i].Distance(cldr).distance) <= 0.1) // Chooses the planet that you are on
                {
                    Vector2 direction = transform.position - Planets[i].transform.position; // Creates vector in planet's direction

                    Vector2 fixedDirection = Rotate(direction, 270f); // Utilizes earlier method to rotate the vector 270 degrees to get everything aligned

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
