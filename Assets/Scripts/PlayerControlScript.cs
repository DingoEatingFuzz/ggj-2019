using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{

    //Declarations
    public Rigidbody2D rb2d;
    public float speed = .05F;
    public float jumpSpeed = 3;
    private bool onGround = false;
    public bool hasFirstKey = false;
    public bool hasDoubleJump = false;
    public bool playerCanDoubleJump = false;
    public bool hasSecondKey = false;
    public bool hasHornPunch = false;
    public bool hasShield = false;
    public bool hasThirdkey = false;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region movement
        float horzMovement = Input.GetAxis("horzAxis")*speed;

        Vector2 movement = new Vector2 (horzMovement, 0);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        //Slow the movement when in the air.
        if(onGround){
            rb2d.AddForce (movement * speed);
        }else{rb2d.AddForce (movement * (speed/3));}
        
        #endregion
        
        #region Action handling
        //Jumping Logic
        if(Input.GetButtonDown("aButton") && (onGround || (playerCanDoubleJump && hasDoubleJump)))
        {
            Vector3 jumpMovement = new Vector3 (0.0f, 1.0f,0.0f);
            rb2d.velocity = jumpMovement * jumpSpeed;

            if(!onGround){
                playerCanDoubleJump = false;
            }

        }

        //Punch Logic
        if(Input.GetButtonDown("bButton") && hasHornPunch){

        }

        //Shield Logic
        if(Input.GetButtonDown("xButton") && hasShield){

        }
        #endregion

    }

    void OnTriggerEnter2D (Collider2D collision){
        if(collision.gameObject.CompareTag("firstKey")){
            hasFirstKey = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("secondKey")){
            hasSecondKey = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("thirdKey")){
            hasThirdkey = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("doubleJumpItem")){
            hasDoubleJump = true;
            playerCanDoubleJump = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("punchItem")){
            hasHornPunch = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("shieldItem")){
            hasShield = true;
            Destroy(collision.gameObject);
        }


    }

    //Check if the object is on the ground
    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            if(hasDoubleJump){
                playerCanDoubleJump = true;
            }

            onGround = true;
        }
    }
 
    //Check if the object has left the ground
    void OnCollisionExit2D (Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }
}
