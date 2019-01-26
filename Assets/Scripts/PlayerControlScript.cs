using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{

    //Declarations
    public Rigidbody2D rb2d;
    public float speed = .05F;
    public float rotateSpeed = 100f;
    public float jumpSpeed = 3;
    private bool onGround = false;
    public bool hasFirstKey = false;
    public bool hasDoubleJump = false;
    public bool playerCanDoubleJump = false;
    public bool hasSecondKey = false;
    public bool hasHornPunch = false;
    public bool hasShield = false;
    public bool hasThirdkey = false;
    private bool punching = false;
    public SpriteRenderer spriteRender;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region movement
        float horzMovement = Input.GetAxis("horzAxis")*speed;
        Vector2 movement = new Vector2 (horzMovement, 0);

        if(rb2d.velocity.x >= 0.1){
            //Debug.Log("Gina should be looking right.");
            spriteRender.flipX = false;
            }
            else {
            //Debug.Log("Gina should be looking left.");    
            spriteRender.flipX = true;
            }

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
        if(Input.GetButtonDown("bButton") && hasHornPunch && !punching){
           // Punch(0.5f, 1.25f, transform.forward);
        }

        //Shield Logic
        if(Input.GetButtonDown("xButton") && hasShield){

        }
        #endregion

    }

   public void Punch(float time, float distance, Vector3 direction){
        var origin = transform.position;
        var offset = transform.right * 1.13f;
        var hits = new HashSet<RaycastHit2D>();

    }

    //This method is where we "kill" the player.
    //they should flash and then be teleported to the the last EXIT the went through.
    public void playerDeath(){

        StartCoroutine(Flash());



        //Destroy(gameObject);
    }

    IEnumerator Flash()
    {
        for (int n = 0; n < 5; n++)
        {
            spriteRender.enabled=false;
            yield return new WaitForSeconds(0.1f);
            spriteRender.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
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
        if(collision.gameObject.CompareTag("enemy")){
            playerDeath();
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
