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
    public bool shieldIsActive = false;
    public bool canActivateShield = false;
    public bool hasThirdkey = false;
    private bool punching = false;
    public SpriteRenderer gina;
    public SpriteRenderer shield;
    private int shieldOnTime = 5;
    private int shieldCoolDown = 7;
    public Animator anim;
    public GameData gameData;
    public bool faceingRight = true;


    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("started player");
        gameData = gameObject.AddComponent<GameData>();
        rb2d = GetComponent<Rigidbody2D>();
        gina = gameObject.GetComponent<SpriteRenderer>();
        shield = transform.Find("tempShield").gameObject.GetComponent<SpriteRenderer>();
        shield.enabled = false;
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region movement
        float horzMovement = Input.GetAxis("horzAxis")*speed;
        Vector2 movement = new Vector2 (horzMovement, 0);

        if(rb2d.velocity.x > 0.25)
        {
            gina.flipX = false;
            faceingRight = true;
            anim.SetTrigger("GinaWalk");
        }
        else if(rb2d.velocity.x < -0.25) 
        {
            gina.flipX = true;
            faceingRight = false;
            anim.SetTrigger("GinaWalk");
        }
        else anim.SetTrigger("GinaIdle");




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

        if(Input.GetButtonDown("aButton")){
            anim.SetBool("Jumping",true);
        }

        //Punch Logic
        if(Input.GetButtonDown("bButton") && onGround ){//&& hasHornPunch && !punching){
           Punch();
           anim.SetTrigger("GinaPunch");
        }

        //Shield Logic
        if(Input.GetButtonDown("xButton") && hasShield){

            if(canActivateShield){
                StartCoroutine(ActivateShield());
            }
        }
        #endregion
    
    }

   public void Punch(){        
        RaycastHit2D hit;
        if(faceingRight)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.right);
        }
        else hit = Physics2D.Raycast(transform.position, Vector2.left);
        if (hit.collider != null && hit.distance < .8 && hit.collider.gameObject.CompareTag("breakableWall"))
        {  
          Destroy(hit.collider.gameObject);
        }
    }

    //This method is where we "kill" the player.
    //they should flash and then be teleported to the the last EXIT the went through.
    public void playerDeath(){
        StartCoroutine(Flash());
        //Teleport to the last exit we went through.
    }

    IEnumerator ActivateShield(){
        shieldIsActive = true;
        shield.enabled = true;
        canActivateShield = false;
        yield return new WaitForSecondsRealtime(shieldOnTime);
        shield.enabled = false;
        shieldIsActive = false;
        yield return new WaitForSecondsRealtime(shieldCoolDown);
        canActivateShield = true;
    }

    IEnumerator Flash()
    {
        for (int n = 0; n < 5; n++)
        {
            gina.enabled=false;
            yield return new WaitForSecondsRealtime(0.1f);
            gina.enabled = true;
            yield return new WaitForSecondsRealtime(0.1f);
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
            canActivateShield = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("enemy") && !shieldIsActive){
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
            anim.SetBool("Jumping",false);
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
