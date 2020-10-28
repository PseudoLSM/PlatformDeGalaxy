using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;



public class PlanetGravity : MonoBehaviour
{
    public float PullRadius;
    public float GravitationalPull;
    public float MinRadius;
    public float DistanceMultiplier;
    
    public LayerMask LayersToPull;
    
    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, PullRadius, LayersToPull);

        for (var i = 0; i < colliders.Length; i++)
        {
            Rigidbody2D rb = colliders[i].GetComponent<Rigidbody2D>();

            Rigidbody2D planet = GetComponent<Rigidbody2D>();

            if (rb == null) continue;
            
            Vector2 direction = transform.position - colliders[i].transform.position;

            if (direction.magnitude < MinRadius) continue;

            float distance = direction.sqrMagnitude * DistanceMultiplier + 1;

            rb.AddForce(direction.normalized * ((GravitationalPull * planet.mass) / distance) * rb.mass * Time.fixedDeltaTime);
        }
    }
}

