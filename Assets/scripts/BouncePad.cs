using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField]
    public float BounceVal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.gameObject;
        Debug.Log("Object Type: " + other.GetType());
        var player = other.GetComponent<Player2>();
        if (player != null)
        {
            player.moveEffect = "BounceStart";
            player.moveEffectTimer = .5f;
            
        }
        
    }
}
