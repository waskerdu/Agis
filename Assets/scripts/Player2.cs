using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public int playerNum = 0; // used to select input source, player color
    public float speed = 10.0f;
    public float jumpPower = 10.0f;
    public Vector2 dashPower = new Vector2(30f,10f);
    public float dashSpeed = 10.0f;
    public float dashTime = 0.3f;
    public float dashClock = 0.0f; //greater than zero during player dash, overrides all movement
    public int maxJumps = 1;
    public int currentJumps = 0;
    bool grounded = false; //set to false every frame, reset to true by oncollsision stay
    Rigidbody2D rb;
    SpriteRenderer rend;
    bool spriteFacingLeft = true;
    public bool canDash = false;
    Light lig;
    GameObject frame;
    string fire = "Fire";
    string horizontal = "Horizontal";
    string vertical = "Vertical";
    string jump = "Jump";
    Color[] playerColors = {Color.red, Color.blue, Color.green, Color.yellow};
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        frame = transform.GetChild(0).gameObject;
        rend = frame.GetComponent<SpriteRenderer>();
        lig = frame.GetComponent<Light>();
        lig.color = playerColors[playerNum];

        if (playerNum > 0)
        {
            fire += playerNum.ToString();
            horizontal += playerNum.ToString();
            vertical += playerNum.ToString();
            jump += playerNum.ToString();
        }
    }

    void Update()
    {
        // begin
        rb.gravityScale = 4.0f;
        Vector2 vel = rb.velocity;

        //dashing
        if (Dash(vel)) return;

        // movement
        vel.x = Input.GetAxisRaw(horizontal) * speed;

        // jumping
        if(grounded)
        {
            currentJumps = maxJumps;
        }
        if(currentJumps > 0)
        {
            if(Input.GetButtonDown(jump))
            {
                vel.y = jumpPower;
                currentJumps--;
            }
        }

        // air control
        if(Input.GetButton(jump))
        {
            rb.gravityScale = 1.0f;
        }

        // sprite stuff
        if(vel.x < 0.0f){spriteFacingLeft = true;}
        if(vel.x > 0.0f){spriteFacingLeft = false;}
        rend.flipX = spriteFacingLeft;

        // wrap up
        rb.velocity = vel;
        grounded = false;
    }

    //returns true if dashing, false if not dashing
    private bool Dash(Vector2 vel)
    {
        // if we are in the middle of a dash
        if (dashClock > 0.0f)
        {
            rb.gravityScale = 0.0f;
            dashClock -= Time.deltaTime;
            rb.velocity = vel;
            lig.enabled = true;
            return true;
        }
        else
        {
            lig.enabled = false;
            frame.transform.eulerAngles = Vector2.zero;
        }

        //if we are starting a dash this frame
        if (Input.GetButtonDown(fire) && canDash)
        {
            canDash = false;
            dashClock = dashTime;
            rb.gravityScale = 0.0f;


            vel.x = Input.GetAxisRaw(horizontal);
            //no x defaults sprite direction
            if (vel.x == 0.0f)
            {
                if (spriteFacingLeft) { vel.x = -1.0f; }
                else { vel.x = 1.0f; }
            }
            else 
            {
                vel.x = vel.x / Mathf.Abs(vel.x);//sets vel.x to either 1 or -1 based on input
            }


            vel.y = Input.GetAxisRaw(vertical);
            //no y defaults to up
            if (vel.y == 0.0f)
            {
                vel.y = 1.0f;
            }
            else 
            {
                vel.y = vel.y / Mathf.Abs(vel.y);// 1 or -1 y
            }
            vel = vel.normalized;
            vel.x *= dashPower.x;
            vel.y *= dashPower.y;
            
            //transform.position += Vector3.up * 0.1f; //why? this breaks the physics engine
            
            rb.velocity = vel;
            float frameAngle = -Vector2.SignedAngle(Vector2.right, vel);
            Debug.Log(frameAngle);
            frame.transform.eulerAngles = Vector3.forward * (frameAngle); //what is this doing??
            return true;
        }

        return false;
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        foreach (ContactPoint2D contact in collisionInfo.contacts)
        {
            if( Vector2.Dot(contact.normal, Vector2.up) > 0.7f )
            {
                grounded = true;
                dashClock = 0.0f;
                canDash = true;
            }
        }
    }
}
