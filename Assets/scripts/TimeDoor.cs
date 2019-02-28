using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDoor : MonoBehaviour
{
    //public bool playerInside = false;
    public GameObject sliderObject;
    public float holdTime = 3.0f;
    public float holdClock = 0.0f;
    public string message = "MoveUp";
    public int collidingPlayers = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(collidingPlayers > 0)
        {
            //
            if(holdClock > holdTime){
                SendMessageUpwards(message);
                holdClock = 0;
                return;
            }
            holdClock += Time.deltaTime;
            sliderObject.transform.GetComponent<Slider>().value = holdClock/holdTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            collidingPlayers++;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            collidingPlayers--;
        }
    }
}
