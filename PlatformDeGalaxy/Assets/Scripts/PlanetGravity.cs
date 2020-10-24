using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Scripts
{
    public class PlanetGravity : MonoBehaviour
    {
        public float PullRadius;
        public float GravitationalPull;
        public float MinRadius;
        public float DistanceMultiplier;

        public LayerMask LayersToPull;

        void FixedUpdate()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, PullRadius, LayersToPull);

            foreach (var collider in colliders)
            {
                Rigidbody2D rb;// = collider.GetComponent<Rigidbody2D>();

                if (rb = null) continue;

                Vector2 direction = transform.position - collider.transform.position;

                if (direction.magnitude < MinRadius) continue;

                float distance = direction.sqrMagnitude * DistanceMultiplier + 1;

                rb.AddForce(direction.normalized * (GravitationalPull / distance) * rb.mass * Time.fixedDeltaTime);
            }

        }
    }
}
