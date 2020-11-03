using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    public GameObject StrongestPlanet;

    protected Rigidbody2D rb;
    protected Collider2D cldr;
    protected GameObject Player;
    public float offset;
    protected Vector3 targetPos;
    protected Vector3 thisPos;
    protected float angle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cldr = GetComponent<Collider2D>();
        Player = gameObject;
        offset = 90;
    }

    void Update()
    {
        //Player.transform.LookAt(Player.GetComponent<PlayerController>().StrongestPlanet.transform.position);
        targetPos = StrongestPlanet.transform.position;
        thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        //Player.transform.LookAt(StrongestPlanet.GetComponent<Transform>().position);
    }

    void FixedUpdate()
    {
        Collider2D[] Planets = Physics2D.OverlapCircleAll(transform.position, ScanRadius, LayerToCheck); // Finds planets withing range ```ScanRadius```

        float[] PlanetGravities = new float[Planets.Length]; // Creates array that will be filled with the magnitudes of the gravities from planets within the scan radius

        if (Planets.Length != 0)
        {
            for (var i = 0; i < Planets.Length; i++)
            {
                PlanetGravities[i] = Planets[i].GetComponent<PlanetGravity>().ForceOfGravity.magnitude; // Populates the empty array with the needed data
            }
        
            float Max = PlanetGravities[0];  
            
            for (var i = 0; i < PlanetGravities.Length; i++) // Finds the largest gravity magnitude
            {
                if (Max < PlanetGravities[i]) Max = PlanetGravities[i] ;
            }
        
            for (var i = 0; i < PlanetGravities.Length; i++) // Assigns the Planet with the strongest gravity acting on the player to the ```StrongestPlanet``` variable
            {
                if (Max == PlanetGravities[i])
                {
                    StrongestPlanet = Planets[i].gameObject;
                }
            }
        }

        if (Input.GetAxis("Horizontal") != 0) // Checks for horizontal movement input
        {
            if (Mathf.Abs(StrongestPlanet.GetComponent<Collider2D>().Distance(cldr).distance) <= 0.1) // Chooses the planet that you are on      THIS WILL BE REMOVED
            {
                Vector2 direction = transform.position - StrongestPlanet.GetComponent<Collider2D>().transform.position; // Creates vector in planet's direction

                Vector2 fixedDirection = RotateVector(direction, 270f); // Utilizes earlier method to rotate the vector 270 degrees to get everything aligned

                rb.AddForce(fixedDirection.normalized * Mathf.Sign(Input.GetAxis("Horizontal")) * xAcceleration * Time.deltaTime); // Adds the force to accelerate the player character
            }
        }
    }
}
