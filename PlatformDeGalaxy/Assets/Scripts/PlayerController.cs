﻿using System.Collections;
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
    // Movement
    [SerializeField] protected float xAcceleration;
    [SerializeField] protected float jumpAcceleration;

    // Jumping
    [SerializeField] protected Transform jumpCheckPoint;
    [SerializeField] protected LayerMask jumpCheckLayer;
    [SerializeField] protected float jumpCheckRadius;
    protected int jumpTokens;
    protected int jumpTokensMax;
    protected bool jumpTokensResetDoor1;
    protected bool jumpTokensResetDoor2;
    protected bool jumpTokensResetDoor3;
    [HideInInspector] public bool isTouchingGround;

    // Planet Checking
    [SerializeField] protected LayerMask PlanetLayer;
    [SerializeField] protected float ScanRadius;
    public GameObject StrongestPlanet;

    protected Rigidbody2D rb;
    protected Collider2D cldr;
    protected GameObject Player;
    protected float offset;
    protected Vector3 targetPos;
    protected Vector3 thisPos;
    protected float angle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cldr = GetComponent<Collider2D>();
        Player = gameObject;
        offset = 90f;
        jumpTokens = 0;
        jumpTokensMax = 1;
        jumpTokensResetDoor1 = true;
        jumpTokensResetDoor2 = true;
        jumpTokensResetDoor3 = true;
    }

    void Update()
    {
        // Fix player rotation to planet
        targetPos = StrongestPlanet.transform.position;
        thisPos = transform.position;
        targetPos.x -= thisPos.x;
        targetPos.y -= thisPos.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));


        isTouchingGround = Physics2D.OverlapCircle(jumpCheckPoint.position, jumpCheckRadius, jumpCheckLayer); // Checks if the player is on top of an object

        if (jumpTokens < jumpTokensMax && isTouchingGround && jumpTokensResetDoor2 == true) // Refreshes jump tokens when on an/the object/ground
        {
            jumpTokens = jumpTokensMax;
            jumpTokensResetDoor1 = false;
            jumpTokensResetDoor2 = false;
            jumpTokensResetDoor3 = false;
        }

        if (jumpTokensResetDoor2 == true)
        {
            jumpTokensResetDoor3 = true;
        }

        if (jumpTokensResetDoor1 == true)
        {
            jumpTokensResetDoor2 = true;
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumpTokens > 0) // Checks for Jump input 
        {
            jumpTokens--;
            jumpTokensResetDoor1 = true;
            Vector2 jumpDirection = transform.position - StrongestPlanet.GetComponent<Collider2D>().transform.position;
            Vector2 fixedJumpDirection = RotateVector(jumpDirection, 180f);
            rb.AddForce(jumpDirection.normalized * Mathf.Sign(fixedJumpDirection.magnitude) * jumpAcceleration);
        }
        Debug.Log(jumpTokens);
    }

    void FixedUpdate()
    {
        Collider2D[] Planets = Physics2D.OverlapCircleAll(transform.position, ScanRadius, PlanetLayer); // Finds planets withing range ```ScanRadius```

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
            if (Mathf.Abs(StrongestPlanet.GetComponent<Collider2D>().Distance(cldr).distance) <= 0.05) // Chooses the planet that you are on      THIS WILL BE REMOVED
            {
                Vector2 moveDirection = transform.position - StrongestPlanet.GetComponent<Collider2D>().transform.position; // Creates vector in planet's direction

                Vector2 fixedMoveDirection = RotateVector(moveDirection, 270f); // Utilizes earlier method to rotate the vector 270 degrees to get everything aligned

                rb.AddForce(fixedMoveDirection.normalized * Mathf.Sign(Input.GetAxis("Horizontal")) * xAcceleration * Time.deltaTime); // Adds the force to accelerate the player character
            }
        }
    }
}
