using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayer : MonoBehaviour
{
    public static PlatformPlayer instance;

    public PlayerHP hp;
    public ParticleSystem bloodyDeath;

    public CapsuleCollider collider;
    public LayerMask jumpableLayers;
    public float strikeThreshold = 3f;

    public float moveSpeed;
    public float jumpSpeed;
    public float jumpDecay = 0.1f;
    public Vector3 jumpCheckBox = new Vector3(.9f, .3f, .1f);
    public float jumpCheckOffset = 0.6f;
    public float stopJumpSpeed;

    public bool struck;

    Vector3 startScale;

    Vector2 movement;

    Rigidbody rb;

    public bool crouching;
    public bool canJump;
    public bool jumping;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
        if (Input.GetAxis("Horizontal") < 0)
        {
            movement = Vector2.left;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            movement = Vector2.right;
        }
        rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, 0f);

        canJump = Physics.OverlapBox(transform.position - Vector3.up * jumpCheckOffset,
                        jumpCheckBox, transform.localRotation, jumpableLayers).Length > 0;

        if (Input.GetAxis("Vertical") > 0 && canJump && !jumping)
        {
            //jump
            jumping = true;
            StartCoroutine(Jump());
        }
        else if (Input.GetAxis("Vertical") < 0)
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

        if (jumping)
        {
            if(Input.GetAxis("Vertical") < 0 || rb.velocity.y < stopJumpSpeed)
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

    IEnumerator Jump()
    {
        float stopTime = Time.time + 0.5f;

        float jSpeed = jumpSpeed;
        rb.AddForce(jSpeed * Vector3.up, ForceMode.Impulse);

        while (Time.time <= stopTime && Input.GetAxis("Vertical") > 0 && jSpeed > 0)
        {
            jSpeed -= jumpDecay;
            rb.AddForce(jSpeed * Vector3.up, ForceMode.Force);
            yield return new WaitForSeconds(0.1f);
        }

        jumping = false;
    }

    Vector3 toV3(Vector2 v2)
    {
        return new Vector3(v2.x, v2.y, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (struck)
            return;

        List<ContactPoint> cp = new List<ContactPoint>();
        int contacts = collision.GetContacts(cp);

        if (contacts == 0)
            return;

        for(int i = 0; i < cp.Count; i++)
        {
            if (cp[i].point.y < transform.position.y )
                return;
        }

        if ( collision.collider.GetComponent<Rigidbody>())
        {
            if (collision.collider.tag == "Piece" 
                && collision.collider.GetComponent<Rigidbody>().velocity.magnitude >= strikeThreshold)
            {
                //collider.enabled = false;
                StartCoroutine(Struck());
            }
        }
        else
        {
            if (collision.collider.tag == "Piece"
                && collision.collider.GetComponentInParent<Rigidbody>().velocity.magnitude >= strikeThreshold)
            {
                //collider.enabled = false;
                StartCoroutine(Struck());
            }
        }
        
    }

    IEnumerator Struck()
    {
        struck = true;

        hp.curHP -= 1;

        if (hp.curHP <= 0)
        {
            GetComponent<MeshRenderer>().enabled = false;
            bloodyDeath.Play();
            yield return new WaitForSeconds(bloodyDeath.main.duration);

            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver",
                 UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        for (int i = 0; i < 3; i++)
        {
            GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(0.15f);
            GetComponent<MeshRenderer>().enabled = true;
            yield return null;
        }

        yield return new WaitForSeconds(0.75f);

        struck = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position - Vector3.up * jumpCheckOffset, jumpCheckBox);
    }
}
