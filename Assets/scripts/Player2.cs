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

        //gathering inputs for frame
        bool jumpDown, jumpPressed, dashDown, dashPressed;
        jumpPressed = Input.GetKeyDown(jump);
        jumpDown = Input.GetKey(jump);
        dashPressed = Input.GetKeyDown(dash);
        dashDown = Input.GetKey(dash);

        //check if player is ready to dash / is trying to dash
            //if so, set moveEffect to dash, set moveEffectTimer to dashTime

        //check if moveEffectTimer has time on it, if so do moveEffect switch statement
            //each move effect should set rb.velocity and return out

        //do player movement based on inputs

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
