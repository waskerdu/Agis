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
    public Vector2 externalVelocity = new Vector2();
    public float dashTime = 0.3f;
    public float dashClock = 0.0f; //greater than zero during player dash, overrides all movement
    public float velocityDecayRate = 10f;
    public float velocityCutoff = 1f;
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
        rb.gravityScale = 10.0f;
        Vector2 vel;
        Vector2 moveVel = new Vector2(0,-1f); //used to keep track of where the player is trying to go
        DecayExternalVelocity();

        //dashing
        if (Dash()) return;

        // movement
        moveVel.x = Input.GetAxisRaw(horizontal) * speed;

        // jumping
        if(grounded)
        {
            currentJumps = maxJumps;
        }
        if(currentJumps > 0)
        {
            if(Input.GetButtonDown(jump))
            {
                moveVel.y = jumpPower;
                currentJumps--;
            }
        }

        // air control
        if(Input.GetButton(jump))
        {
            rb.gravityScale = 1.0f;
        }

        // sprite stuff
        if(moveVel.x < 0.0f){spriteFacingLeft = true;}
        if(moveVel.x > 0.0f){spriteFacingLeft = false;}
        rend.flipX = spriteFacingLeft;

        // wrap up
        vel = moveVel + externalVelocity;
        rb.velocity = moveVel;
        grounded = false;
    }

    //returns true if dashing, false if not dashing
    private bool Dash()
    {
        Vector2 dashVel = new Vector2();
        // if we are in the middle of a dash
        if (dashClock > 0.0f)
        {
            rb.gravityScale = 0.0f;
            dashClock -= Time.deltaTime;
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


            dashVel.x = Input.GetAxisRaw(horizontal);
            //no x defaults sprite direction
            if (dashVel.x == 0.0f)
            {
                if (spriteFacingLeft) { dashVel.x = -1.0f; }
                else { dashVel.x = 1.0f; }
            }
            else 
            {
                dashVel.x = dashVel.x / Mathf.Abs(dashVel.x);//sets dashVel.x to either 1 or -1 based on input
            }


            dashVel.y = Input.GetAxisRaw(vertical);
            //no y defaults to up
            if (dashVel.y == 0.0f)
            {
                dashVel.y = 1.0f;
            }
            else 
            {
                dashVel.y = dashVel.y / Mathf.Abs(dashVel.y);// 1 or -1 y
            }
            dashVel = dashVel.normalized;
            dashVel.x *= dashPower.x;
            dashVel.y *= dashPower.y;
            
            //transform.position += Vector3.up * 0.1f; //why? this breaks the physics engine
            
            rb.velocity = dashVel;
            float frameAngle = -Vector2.SignedAngle(Vector2.right, dashVel);
            Debug.Log(frameAngle);
            frame.transform.eulerAngles = Vector3.forward * (frameAngle); //what is this doing??
            return true;
        }

        return false;
    }

    private void DecayExternalVelocity(){
        externalVelocity *= Time.deltaTime * velocityDecayRate;
        if(externalVelocity.magnitude < velocityCutoff){
            Debug.Log("cutting external velocity");
            externalVelocity = Vector2.zero;
        }
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
