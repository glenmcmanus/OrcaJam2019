using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlayer : MonoBehaviour
{
    public static FallingPlayer instance;

    bool fallPhase;
    bool tetrisPhase;
    bool winPhase;
    bool losePhase;

    Rigidbody rb;
    public float[] fallSpeed = new float[] { 0.001f, 0.002f };
    public float[] acceleration = new float[2];
    public float[] maxVelocity = new float[2];
    public float[] sway = new float[2];
    Vector3 velocity;
    public FallState fallstate;

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
    }

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

        if (Input.GetAxis("Horizontal") > 0){
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

        if(Input.GetButton("Dive"))
        {
            fallstate = FallState.Dive;
        }
        else
        {
            fallstate = FallState.Drag;
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

}

public enum FallState
{
    Drag,
    Dive
}
