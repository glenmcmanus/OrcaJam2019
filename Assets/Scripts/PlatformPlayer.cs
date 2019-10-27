using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayer : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public float boxCastDist;
    public float stopJumpSpeed;

    Vector3 startScale;

    Vector2 movement;

    Rigidbody rb;

    public bool crouching;
    public bool canJump;
    public bool jumping;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movement = Vector2.right;
        }
        rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, 0f);

        canJump = (Physics.OverlapBox(transform.position - Vector3.up * boxCastDist, transform.localScale, transform.localRotation).Length > 1);

        if (Input.GetKeyDown(KeyCode.UpArrow) && canJump)
        {
            //jump
            jumping = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (!crouching) { 
                crouching = true;
                transform.position -= Vector3.up * startScale.y * .5f;
            }
        }
        else if(crouching)
        {
            transform.position += Vector3.up * startScale.y * .5f;
            crouching = false;
        }

        if(jumping)
        {
            if(!Input.GetKey(KeyCode.UpArrow) || rb.velocity.y < stopJumpSpeed)
            {
                jumping = false;
                rb.velocity = new Vector3(rb.velocity.x, stopJumpSpeed, rb.velocity.z);
            }
        }



        if (crouching)
        {
            transform.localScale = new Vector3(startScale.x, startScale.y * .5f, startScale.z);
        }
        else
        {
            transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z);
        }



    }

    Vector3 toV3(Vector2 v2)
    {
        return new Vector3(v2.x, v2.y, 0f);
    }
}
