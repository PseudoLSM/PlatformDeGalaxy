using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;



public class PlanetGravity : MonoBehaviour
{
    [SerializeField] protected float PullRadius;
    [SerializeField] protected float GravitationalPull;
    [SerializeField] protected float MinRadius;
    [SerializeField] protected float DistanceMultiplier;

    [SerializeField] protected GameObject PlayerObject;

    [HideInInspector] public Vector2 ForceOfGravity = new Vector2(0, 0);
    
    [SerializeField] protected LayerMask LayersToPull;
    
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

            if (colliders[i].gameObject == PlayerObject)
            {
                ForceOfGravity = direction.normalized * ((GravitationalPull * planet.mass) / distance) * rb.mass * Time.fixedDeltaTime;

                if (ForceOfGravity == PlayerObject.GetComponent<PlayerController>().StrongestPlanet.GetComponent<PlanetGravity>().ForceOfGravity)
                {
                    rb.AddForce(direction.normalized * ((GravitationalPull * planet.mass) / distance) * Time.fixedDeltaTime);
                }
            } 
            else
            {
                rb.AddForce(direction.normalized * ((GravitationalPull * planet.mass) / distance) * Time.fixedDeltaTime);
            }
        }
    }
}

