using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour {

    public float jumpPower = 10.0f;
    public float hspeed = 10.0f;
    public float airControl = 0.5f;

    public float dashSpeed = 20.0f;
    public float dashTime = 0.3f;
    public float dashClock = 0.0f;
    public int defaultDashes = 1;
    public int maxDashes = 1;
    public int dashes = 1;

    public bool isGrounded = false;
    public bool isDashing = false;
    public bool isSliding = false;

    public int jumps = 1;
    public int maxJumps = 1;
    public int defaultJumps = 1;
    public int defaultGravity = 2;
    public int floatGravity = 1;
    public int controller = 0;
    public int points = 0;
    public float sloMo = 0.25f;
    public Text text;

    Vector3 startpos;

    string hstring, vstring, firestring, jumpstring;

    Vector3 velocity;
    Rigidbody2D rb;
    RaycastHit2D hit;
    GameObject hitbox;
    GameObject damageBoxTop, damageBoxBottom;
    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        startpos = transform.position;
        jumps = defaultJumps;
        maxJumps = defaultJumps;
        dashes = defaultDashes;
        maxDashes = defaultDashes;
        hitbox = transform.GetChild(0).gameObject;
        damageBoxTop = hitbox.transform.GetChild(0).gameObject;
        damageBoxBottom = hitbox.transform.GetChild(1).gameObject;
        hstring = "Horizontal";
        vstring = "Vertical";
        firestring = "Fire";
        jumpstring = "Jump";
        if(controller == 0)
        {
            hstring = "Horizontal";
            vstring = "Vertical";
            firestring = "Fire";
            jumpstring = "Jump";
        }
        else
        {
            hstring = "Horizontal1";
            vstring = "Vertical1";
            firestring = "Fire1";
            jumpstring = "Jump1";
        }
    }

    void Point()
    {
        points++;
        text.text = "Points: " + points;
        transform.position = startpos;
        rb.velocity *= 0.0f;
        dashClock = 0.0f;
    }

	void OnCollisionEnter2D(Collision2D col)
    {
        dashClock = 0.0f;
       /**/
        //need to add more for various bounce pads
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "DamageBox")
        {
            col.gameObject.SendMessageUpwards("Point");
            transform.position = startpos;
            rb.velocity *= 0.0f;
            dashClock = 0.0f;
        }
    }
	// Update is called once per frame
	void Update () {
        hit = Physics2D.Raycast(transform.position, Vector2.down, 1.3f);
        //Debug.Log(hit.distance);
        isGrounded = hit.collider != null;
        if (Input.GetButtonDown(firestring) && dashes > 0)
        {
            if(Input.GetAxis(hstring) != 0.0f)
            {
                dashClock = dashTime;
                velocity *= 0;
                dashes--;
                if (Input.GetAxis(hstring) < 0.0f) { velocity.x = -1; }
                if (Input.GetAxis(hstring) > 0.0f) { velocity.x = 1; }
                if (Input.GetAxis(vstring) < 0.0f) { velocity.y = -1; }
                if (Input.GetAxis(vstring) > 0.0f) { velocity.y = 1; }
                if (velocity.y == 0.0f) { velocity.y = 1.0f; }//will dash up by default
                transform.rotation = Quaternion.identity;
                if (velocity.y == 1)
                {
                    transform.Rotate(Vector3.back, velocity.x * 30);
                    damageBoxTop.SetActive(true);
                }
                else
                {
                    damageBoxBottom.SetActive(true);
                    if (velocity.x < 0.0f)
                    {
                        transform.Rotate(Vector3.back, -velocity.x * 10); //ugly hack, don't know why this one direction is wrong
                    }
                    else
                    {
                        transform.Rotate(Vector3.back, -velocity.x * 30);
                    }
                }
                if (velocity.x<0.0f && velocity.y < 0.0f) { transform.Rotate(Vector3.forward * -30); }
                isDashing = true;
                rb.gravityScale = 0.0f;
                rb.velocity = velocity.normalized * dashSpeed;
                Time.timeScale = sloMo;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }
        }
        if(!isSliding && isGrounded && rb.velocity.y < 0.0f) { dashClock = 0.0f; }
        if (Input.GetButtonDown(jumpstring))
        {
            dashClock = 0.0f;
        }
        if (dashClock > 0.0f)
        {
            dashClock -= Time.deltaTime;
            return;
        }
        else if (isDashing)
        {
            isDashing = false;
            rb.gravityScale = defaultGravity;
            transform.rotation = Quaternion.identity;
            damageBoxTop.SetActive(false);
            damageBoxBottom.SetActive(false);
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        velocity = rb.velocity;
        velocity.x = Input.GetAxis(hstring) * hspeed;
        if (isGrounded || jumps > 0)
        {
            if (Input.GetButtonDown(jumpstring))
            {
                //if (velocity.y < 0.0f) { velocity.y = 0; }
                velocity.y = jumpPower;
                jumps--;
                rb.gravityScale = floatGravity;
            }
            
        }
        if (Input.GetButtonUp(jumpstring))
        {
            rb.gravityScale = defaultGravity;
        }
        if (isGrounded)
        {
            jumps = maxJumps;
            dashes = maxDashes;
            //Debug.Log("got here");
        }
        else
        {
            velocity.x *= airControl;
        }
        rb.velocity = velocity;
        
	}
}
