using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public string moveEffect = "";
    public float moveEffectTimer = 0f;
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
    string dash = "Fire";
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
        SetInputs();
    }


     public void SetInputs(bool xbox = true)
    {
        string playerNumStr = playerNum.ToString();
        if(playerNum == 0)
        {
            dash = "Fire";
            jump = "Jump";
            horizontal = "Horizontal";
            vertical = "Vertical";
        }
        else if (xbox)
        {
            dash = "joystick " + playerNumStr + " button 4";
            horizontal = "Horizontal" + playerNumStr;
            vertical = "Vertical" + playerNumStr;
            jump = "joystick " + playerNumStr + " button 0";
        }
        else
        {
            dash = "joystick " + playerNumStr + " button 6"; 
            horizontal = "Horizontal" + playerNumStr;
            vertical = "Vertical" + playerNumStr;
            jump = "joystick " + playerNumStr + " button 1";
        }
    }
    void Update()
    {
        rb.gravityScale = 4.0f;
        Vector2 vel = rb.velocity;

        //gathering inputs for frame
        bool jumpDown, jumpPressed, dashDown, dashPressed;
        jumpPressed = Input.GetButtonDown("Jump");
        jumpDown = Input.GetButton("Jump");
        dashPressed = Input.GetButtonDown("Fire");
        dashDown = Input.GetButton("Fire");

        //check if player is ready to dash / is trying to dash
            //if so, set moveEffect to dash, set moveEffectTimer to dashTime

        //check if moveEffectTimer has time on it, if so do moveEffect switch statement
            //each move effect should set rb.velocity and return out

        //do player movement based on inputs
                // x movement
        vel.x = Input.GetAxisRaw(horizontal) * speed;

               // jumping
        if(grounded)
        {
            currentJumps = maxJumps;
        }
        if(currentJumps > 0)
        {
            if(jumpPressed)
            {
                vel.y = jumpPower;
                currentJumps--;
                grounded = false;
            }
        }

                // air control
        if(jumpDown)
        {
            rb.gravityScale = 1.0f;
        }

        // sprite stuff
        if(vel.x < 0.0f){spriteFacingLeft = true;}
        if(vel.x > 0.0f){spriteFacingLeft = false;}
        rend.flipX = spriteFacingLeft;

        // wrap up
        rb.velocity = vel;

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
