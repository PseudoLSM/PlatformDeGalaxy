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
        Collider2D collider = Physics2D.OverlapCircle(transform.position, PullRadius, LayersToPull);

        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

        if (rb = null) return;

        Vector2 direction = transform.position - collider.transform.position;

        if (direction.magnitude > MinRadius) return;

        float distance = direction.sqrMagnitude * DistanceMultiplier + 1;

        rb.AddForce(direction.normalized * (GravitationalPull / distance) * rb.mass * Time.fixedDeltaTime);
       
    }
}

