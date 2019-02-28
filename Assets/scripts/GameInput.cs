using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInput
{
    //
    public enum ControllerType
    {
        aggregate,
        keyboard,
        xbox,
        dualshock,
    }
    public class ControllerManager
    {
        //
    }

    public class Controller
    {
        //
        ControllerType type = ControllerType.xbox;
        ControllerManager manager;
        int index = 0;
        public Controller(ControllerManager manager, ControllerType type = ControllerType.xbox, int index = 0)
        {
            this.manager = manager;
            this.type = type;
            this.index = index;
        }
        public bool JumpPressed()
        {
            switch(type)
            {
                case ControllerType.xbox:
                    return Input.GetKeyDown("joystick "+index.ToString()+ " button 0");
                case ControllerType.dualshock:
                    return Input.GetKeyDown("joystick "+index.ToString()+ " button 1");
                case ControllerType.keyboard:
                    return Input.GetButtonDown("Jump");
            }
            return false;
        }
        public bool JumpDown()
        {
            switch(type)
            {
                case ControllerType.xbox:
                    return Input.GetKey("joystick "+index.ToString()+ " button 0");
                case ControllerType.dualshock:
                    return Input.GetKey("joystick "+index.ToString()+ " button 1");
                case ControllerType.keyboard:
                    return Input.GetButton("Jump");
            }
            return false;
        }
        public bool DashPressed()
        {
            switch(type)
            {
                case ControllerType.xbox:
                    return Input.GetKeyDown("joystick "+index.ToString()+ " button 4");
                case ControllerType.dualshock:
                    return Input.GetKeyDown("joystick "+index.ToString()+ " button 4");
                case ControllerType.keyboard:
                    return Input.GetButtonDown("Dash");
            }
            return false;
        }
        public bool DashDown()
        {
            switch(type)
            {
                case ControllerType.xbox:
                    return Input.GetKey("joystick "+index.ToString()+ " button 4");
                case ControllerType.dualshock:
                    return Input.GetKey("joystick "+index.ToString()+ " button 4");
                case ControllerType.keyboard:
                    return Input.GetButton("Dash");
            }
            return false;
        }
        public float Horizontal()
        {
            return Input.GetAxisRaw("Horizontal"+index.ToString());
        }
        public Vector2 DashDirection()
        {
            Vector2 outVec;
            outVec.x = Input.GetAxisRaw("Horizontal"+index.ToString());
            if(outVec.x>0){outVec.x=1;}
            if(outVec.x<0){outVec.x=-1;}
            outVec.y = Input.GetAxisRaw("Vertical"+index.ToString());
            if(outVec.y>0){outVec.y=1;}
            if(outVec.y<0){outVec.y=-1;}
            return outVec.normalized;
        }
        public bool StartButton(){return true;}
        public bool MenuUp(){return true;}
        public bool MenuDown(){return true;}
        public bool MenuLeft(){return true;}
        public bool MenuRight(){return true;}
    }
}