﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlayer : MonoBehaviour
{
    public static FallingPlayer instance;

    public PlayerHP hp;

    public GameObject postProcessing;

    bool fallPhase;
    bool tetrisPhase;
    bool winPhase;
    bool losePhase;

    public CapsuleCollider collider;
    Rigidbody rb;
    public float[] fallSpeed = new float[] { 0.001f, 0.002f };
    public float pieceDiveSpd = 12;
    public float pieceDragSpd = 6;
    public float[] acceleration = new float[2];
    public float[] maxVelocity = new float[2];
    public float[] sway = new float[2];
    Vector3 velocity;

    [Header("State Vars")]
    public FallState fallstate;
    public float dragPosY = 4.5f;
    public float divePosY = 3f;
    public float fallPosYdelta = 0.1f;
    bool changingDepth;

    [Header("Bounds")]
    public float maxX;
    public float maxZ;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        hp.curHP = hp.maxHP;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StateMachine());
        rb = GetComponent<Rigidbody>();
        velocity = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(0, dragPosY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 change = new Vector3(0f, 0f, 0f);

        if (Input.GetAxis("Horizontal") > 0) {
            change.x += acceleration[(int)fallstate];
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            change.x -= acceleration[(int)fallstate];
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            change.z += acceleration[(int)fallstate];
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            change.z -= acceleration[(int)fallstate];
        }

        if (Input.GetButtonDown("Dive"))
        {
            fallstate = FallState.Dive;
            if (!changingDepth)
            {
                changingDepth = true;
                StartCoroutine(ChangeDepth());
            }

            for (int i = 0; i < SpawnScript.instance.transform.childCount; i++)
            {
                SpawnScript.instance.transform.GetChild(i).GetComponent<Rigidbody>().velocity
                                                            = (pieceDiveSpd * Vector3.up);

               // if (i == 0)
               //     Debug.Log("dive " + SpawnScript.instance.transform.GetChild(i).GetComponent<Rigidbody>().velocity);
            }
        }
        else if(Input.GetButtonUp("Dive"))
        {
            fallstate = FallState.Drag;
            if (!changingDepth)
            {
                changingDepth = true;
                StartCoroutine(ChangeDepth());
            }

            for (int i = 0; i < SpawnScript.instance.transform.childCount; i++)
            {
                SpawnScript.instance.transform.GetChild(i).GetComponent<Rigidbody>().velocity
                                                            = (pieceDragSpd * Vector3.up);

               // if (i == 0)
               //     Debug.Log("drag " + SpawnScript.instance.transform.GetChild(i).GetComponent<Rigidbody>().velocity);
            }
        }

        if(change.magnitude > acceleration[(int)fallstate])
        {
            change *= acceleration[(int)fallstate] / change.magnitude;
        }

        if(change.magnitude == 0 && rb.velocity.magnitude != 0)
        {
            if (rb.velocity.magnitude > acceleration[(int)fallstate])
            {
                change = -rb.velocity.normalized * acceleration[(int)fallstate];
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

        change.x += Random.Range(-sway[(int)fallstate], sway[(int)fallstate]);
        change.z += Random.Range(-sway[(int)fallstate], sway[(int)fallstate]);

        rb.velocity += change;

        if(rb.velocity.magnitude > maxVelocity[(int)fallstate])
        {
            rb.velocity *= maxVelocity[(int)fallstate] / rb.velocity.magnitude;
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

    IEnumerator ChangeDepth()
    {
        while( (fallstate == FallState.Drag && transform.position.y < dragPosY)
               || (fallstate == FallState.Dive && transform.position.y > divePosY) )
        {
            float delta = (((int)fallstate * -2) + 1) * fallPosYdelta;
            transform.Translate(Vector3.back * delta);
            yield return new WaitForEndOfFrame();
        }

        transform.position = fallstate == FallState.Drag ?
                                new Vector3(transform.position.x, dragPosY, transform.position.z) :
                                new Vector3(transform.position.x, divePosY, transform.position.z);

        changingDepth = false;
    }

    IEnumerator Struck()
    {
        postProcessing.GetComponent<VignetteDamage>().Ouch();

        hp.curHP -= 1;

        if (hp.curHP <= 0)
        {
            Debug.Log("YOU ARE DEAD");
        }

        for(int i = 0; i < 3; i++)
        {
            GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(0.15f);
            GetComponent<MeshRenderer>().enabled = true;
            yield return null;
        }

        collider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Piece")
        {
            collider.enabled = false;
            StartCoroutine(Struck());
        }
    }
    
}

public enum FallState
{
    Drag,
    Dive
}
