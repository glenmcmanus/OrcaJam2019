using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{
    bool fallPhase;
    bool tetrisPhase;
    bool winPhase;
    bool losePhase;

    Rigidbody rb;
    public float acceleration;
    public float maxVelocity;
    public float sway;
    Vector3 velocity;

    public float maxX;
    public float maxZ;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StateMachine());
        rb = GetComponent<Rigidbody>();
        velocity = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 change = new Vector3(0f, 0f, 0f);

        if (Input.GetKey("d")){
            change.x += acceleration;
        }
        if (Input.GetKey("a"))
        {
            change.x -= acceleration;
        }
        if (Input.GetKey("w"))
        {
            change.z += acceleration;
        }
        if (Input.GetKey("s"))
        {
            change.z -= acceleration;
        }

        if(change.magnitude > acceleration)
        {
            change *= acceleration / change.magnitude;
        }

        if(change.magnitude == 0 && rb.velocity.magnitude != 0)
        {
            if (rb.velocity.magnitude > acceleration)
            {
                change = -rb.velocity.normalized * acceleration;
            }
            else
            {
                rb.velocity *= 0;
            }
        }

        /*if(change.x == 0 && rb.velocity.x != 0)
        {
            if (Mathf.Abs(rb.velocity.x) > acceleration) {
                change.x = -Mathf.Sign(rb.velocity.x) * acceleration;
            }
            else{
                rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
            }
        }

        if (change.z == 0 && rb.velocity.z != 0)
        {
            if (Mathf.Abs(rb.velocity.z) > acceleration)
            {
                change.z = -Mathf.Sign(rb.velocity.z) * acceleration;
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
            }
        }*/

        change.x += Random.Range(-sway, sway);
        change.z += Random.Range(-sway, sway);

        rb.velocity += change;

        if(rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity *= maxVelocity / rb.velocity.magnitude;
        }

        if(Mathf.Abs(transform.position.x) > maxX)
        {
            transform.position -= new Vector3(transform.position.x - Mathf.Sign(transform.position.x) * maxX, 0f, 0f);
        }
        if (Mathf.Abs(transform.position.z) > maxZ)
        {
            transform.position -= new Vector3(0f, 0f, transform.position.z - Mathf.Sign(transform.position.z) * maxZ);
        }

    }

  
    /*IEnumerator StateMachine()
    {
        while(true)
        {
            while(fallPhase)
            {

            }

            while(tetrisPhase)
            {

            }

            while(winPhase)
            {

            }

            while(losePhase)
            {

            }

        }
    }*/
}
