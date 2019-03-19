using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public int playerNum = 0;
    public float invulnTime = 0.1f;
    public float invlunClock = 0.0f;
    public string hitMessage;
    public float speed = 10.0f;
    public float jumpPower = 10.0f;
    public float dashSpeed = 10.0f;
    public float dashTime = 0.3f;
    public float dashClock = 0.0f;
    public int maxJumps = 1;
    public int currentJumps = 0;
    bool grounded = false;
    bool canJump = false;
    Rigidbody2D rb;
    SpriteRenderer rend;
    bool spriteFacing = true;
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

    public void SetPlayerNum(int playerNum){this.playerNum=playerNum;}

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
        // begin
        rb.gravityScale = 4.0f;
        Vector2 vel = rb.velocity;

        if(invlunClock > 0){invlunClock -= Time.deltaTime;}

        bool jumpDown, jumpPressed, dashDown, dashPressed;
        if(playerNum == 0)
        {
            //
            jumpPressed = Input.GetButtonDown("Jump");
            jumpDown = Input.GetButton("Jump");
            dashPressed = Input.GetButtonDown("Fire");
            dashDown = Input.GetButton("Fire");
        }
        else
        {
            jumpPressed = Input.GetKeyDown(jump);
            jumpDown = Input.GetKey(jump);
            dashPressed = Input.GetKeyDown(dash);
            dashDown = Input.GetKey(dash);
        }

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
        if(dashPressed && canDash)
        {
            rb.gravityScale = 0.0f;
            vel.x = Input.GetAxisRaw(horizontal);
            if(vel.x == 0.0f)
            {
                if(spriteFacing){vel.x = -1.0f;}
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
            if(jumpPressed)
            {
                vel.y = jumpPower;
                currentJumps--;
            }
        }

        // air control
        if(jumpDown)
        {
            rb.gravityScale = 1.0f;
        }

        // sprite stuff
        if(vel.x < 0.0f){spriteFacing = true;}
        if(vel.x > 0.0f){spriteFacing = false;}
        rend.flipX = spriteFacing;

        // wrap up
        rb.velocity = vel;
        grounded = false;
    }

    void SetHitMessage(string message)
    {
        this.hitMessage = message;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //
        if(collider.tag == "DamageBox" && invlunClock > 0 == false)
        {
            // send message
            invlunClock = invulnTime;
            SendMessageUpwards(hitMessage);
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
