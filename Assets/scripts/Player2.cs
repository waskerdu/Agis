using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public int playerNum = 0; // used to select input source, player color
    public float speed = 10.0f;
    public float jumpPower = 10.0f;
    public float dashSpeed = 10.0f;
    public float dashTime = 0.3f;
    public float dashClock = 0.0f; //greater than zero during player dash, overrides all movement
    public int maxJumps = 1;
    public int currentJumps = 0;
    bool grounded = false; //set to false every frame, reset to true by oncollsision stay
    Rigidbody2D rb;
    SpriteRenderer rend;
    bool spriteFacingRight = true;
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


        Dash();
        // dashing
        if (dashClock > 0.0f)
        {
            rb.gravityScale = 0.0f;
            dashClock -= Time.deltaTime;
            rb.velocity = vel;
            lig.enabled = true;
            return;
        }
        else
        {
            lig.enabled = false;
            frame.transform.eulerAngles = Vector2.zero;
        }
        if(Input.GetButtonDown(fire) && canDash)
        {
            rb.gravityScale = 0.0f;
            vel.x = Input.GetAxisRaw(horizontal);
            if(vel.x == 0.0f)
            {
                if(spriteFacingRight){vel.x = -1.0f;}
                else{vel.x = 1.0f;}
            }
            else
            {
                vel.x = vel.x / Mathf.Abs(vel.x);
            }
            vel.y = Input.GetAxisRaw(vertical);
            if(vel.y == 0.0f)
            {
                vel.y = 1.0f;
            }
            else{
                vel.y = vel.y / Mathf.Abs(vel.y);
            }
            vel = vel.normalized * dashSpeed;
            Debug.Log(vel);
            dashClock = dashTime;
            transform.position += Vector3.up * 0.1f;
            canDash = false;
            rb.velocity = vel;
            vel.x *= 3.0f;
            float frameAngle = -Vector2.SignedAngle(Vector2.right, vel);
            Debug.Log(frameAngle);
            frame.transform.eulerAngles = Vector3.forward * (frameAngle);
            return;
        }

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
        if(vel.x < 0.0f){spriteFacingRight = true;}
        if(vel.x > 0.0f){spriteFacingRight = false;}
        rend.flipX = spriteFacingRight;

        // wrap up
        rb.velocity = vel;
        grounded = false;
    }

    private bool Dash(Vector2 vel)
    {
        // dashing
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
        if (Input.GetButtonDown(fire) && canDash)
        {
            rb.gravityScale = 0.0f;
            vel.x = Input.GetAxisRaw(horizontal);
            if (vel.x == 0.0f)
            {
                if (spriteFacingRight) { vel.x = -1.0f; }
                else { vel.x = 1.0f; }
            }
            else
            {
                vel.x = vel.x / Mathf.Abs(vel.x);
            }
            vel.y = Input.GetAxisRaw(vertical);
            if (vel.y == 0.0f)
            {
                vel.y = 1.0f;
            }
            else
            {
                vel.y = vel.y / Mathf.Abs(vel.y);
            }
            vel = vel.normalized * dashSpeed;
            Debug.Log(vel);
            dashClock = dashTime;
            transform.position += Vector3.up * 0.1f;
            canDash = false;
            rb.velocity = vel;
            vel.x *= 3.0f;
            float frameAngle = -Vector2.SignedAngle(Vector2.right, vel);
            Debug.Log(frameAngle);
            frame.transform.eulerAngles = Vector3.forward * (frameAngle);
            return;
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
