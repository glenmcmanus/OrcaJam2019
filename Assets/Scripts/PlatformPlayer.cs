using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayer : MonoBehaviour
{
    public static PlatformPlayer instance;

    public Animator animator;

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

    Vector2 movement;

    Rigidbody rb;

    public bool crouching;
    public bool canJump;
    public bool jumping;

    // Update is called once per frame
    public IEnumerator InputHandler()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        struck = false;
        jumping = false;

        for (; ; )
        {
            movement = Vector2.zero;
            if (Input.GetAxis("Horizontal") < 0)
            {
                movement = Vector2.left;
                animator.SetInteger("run", -1);
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                movement = Vector2.right;
                animator.SetInteger("run", 1);
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
                animator.SetInteger("run", 0);

            rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, 0f);

            canJump = CanJump();
            if(canJump)
            {
                animator.SetBool("grounded", true);
                animator.SetBool("dive", false);
            }

            if (Input.GetAxis("Vertical") > 0 && canJump && !jumping)
            {
                //jump
                jumping = true;
                StartCoroutine(Jump());
            }

            if (jumping)
            {
                if (Input.GetAxis("Vertical") < 0 || rb.velocity.y < stopJumpSpeed)
                {
                    jumping = false;
                    rb.velocity = new Vector3(rb.velocity.x, stopJumpSpeed, rb.velocity.z);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public bool CanJump()
    {
        return Physics.OverlapBox(transform.position - Vector3.up * jumpCheckOffset,
                           jumpCheckBox, transform.localRotation, jumpableLayers).Length > 0;
    }

    IEnumerator Jump()
    {
        animator.SetBool("jump", true);

        float jSpeed = jumpSpeed;
        rb.AddForce(jSpeed * Vector3.up, ForceMode.Impulse);

        while (Input.GetAxis("Vertical") > 0 && jSpeed > 0)
        {
            jSpeed -= jumpDecay;
            rb.AddForce(jSpeed * Vector3.up, ForceMode.Force);
            yield return null;
        }

        jumping = false;

        animator.SetBool("dive", true);
    }

    Vector3 toV3(Vector2 v2)
    {
        return new Vector3(v2.x, v2.y, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enabled || struck)
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
        Debug.Log("platform struck");
        struck = true;

        FallingPlayer.instance.aSource.PlayOneShot(FallingPlayer.instance.hurtSFX);

        hp.curHP -= 1;

        if (hp.curHP <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);

            bloodyDeath.Play();
            yield return new WaitForSeconds(bloodyDeath.main.duration);

            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver",
                 UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.15f);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        struck = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position - Vector3.up * jumpCheckOffset, jumpCheckBox);

        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
