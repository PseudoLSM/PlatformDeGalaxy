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
    protected bool containsPlayer;
    
    [SerializeField] protected LayerMask LayersToPull;
    
    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, PullRadius, LayersToPull);

        for (var i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == PlayerObject)
            {
                containsPlayer = true;
                break;
            }
            else
            {
                continue;
            }
        }

        if (containsPlayer == false) { ForceOfGravity = new Vector2(0, 0); }

        for (var j = 0; j < colliders.Length; j++)
        {
            Rigidbody2D rb = colliders[j].GetComponent<Rigidbody2D>();

            Rigidbody2D planet = GetComponent<Rigidbody2D>();

            if (rb == null) continue;
            
            Vector2 direction = transform.position - colliders[j].transform.position;

            if (direction.magnitude < MinRadius) continue;

            float distance = direction.sqrMagnitude * DistanceMultiplier + 1;

            if (colliders[j].gameObject == PlayerObject)
            {
                ForceOfGravity = direction.normalized * ((GravitationalPull * planet.mass) / distance) * rb.mass * Time.fixedDeltaTime;
                
                if (ForceOfGravity == PlayerObject.GetComponent<PlayerController>().StrongestPlanet.GetComponent<PlanetGravity>().ForceOfGravity)
                {
                    rb.AddForce(direction.normalized * ((GravitationalPull * planet.mass * PlayerObject.GetComponent<Rigidbody2D>().mass) / distance) * Time.fixedDeltaTime);
                }
            } 
            else
            {
                rb.AddForce(direction.normalized * ((GravitationalPull * planet.mass * colliders[j].gameObject.GetComponent<Rigidbody2D>().mass) / distance) * Time.fixedDeltaTime);
            }
        }
    }
}

