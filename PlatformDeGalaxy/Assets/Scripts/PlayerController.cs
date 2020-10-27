using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float xAcceleration;
    public float ScanRadius;

    public LayerMask LayerToCheck;

    Rigidbody2D rb;
    Collider2D cldr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cldr = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0) // Doesnt get past here with FixedUpdate, no fucking clue why.
        {
            Debug.Log(Mathf.Sign(Input.GetAxis("Horizontal"))); // Checker

            Collider2D[] Planets = Physics2D.OverlapCircleAll(transform.position, ScanRadius, LayerToCheck);

            for (var i = 0; i < Planets.Length; i++)
            {
                if (Planets[i] == null) continue;

                Debug.Log(Mathf.Sign(Input.GetAxis("Horizontal"))); //Checker

                if (Mathf.Abs(Planets[0].Distance(cldr).distance) <= 0) // Doesnt get past here with Update, I dont know why yet.
                {
                    Debug.Log(Mathf.Sign(Input.GetAxis("Horizontal"))); // Checker

                    Vector2 direction = transform.position - Planets[i].transform.position;

                    rb.AddForce(direction.normalized * Mathf.Sign(Input.GetAxis("Horizontal")) * xAcceleration * Time.deltaTime);
                }
                else
                {
                    continue;
                }
            }
            /* not needed
            
            if (Mathf.Abs(Planets[0].Distance(cldr).distance) >= Mathf.Abs(Planets[1].Distance(cldr).distance))
            {
                //Check Whichevers Planets Gravity is the strongest and use that planed as the basepoint from which you are going to move left and right on. Or, you could just find how far the edge of the collider is from the character.
            }
            else if (Mathf.Abs(Planets[0].Distance(cldr).distance) < Mathf.Abs(Planets[1].Distance(cldr).distance))
            {

            }
            */
        }
    }
}
