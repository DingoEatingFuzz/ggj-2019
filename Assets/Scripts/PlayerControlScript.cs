﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{

    //Declarations
    public Rigidbody2D rb2d;
    public float speed = 5f;
    public float jumpSpeed = 8;
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

    public SpriteRenderer shieldIcon;
    public SpriteRenderer gloveIcon;
    public SpriteRenderer featherIcon;
    public SpriteRenderer key1Icon;
    public SpriteRenderer key2Icon;
    public SpriteRenderer key3Icon;

    private int shieldOnTime = 5;
    private int shieldCoolDown = 7;
    public Animator anim;
    public GameData gameData;
    public bool faceingRight = true;
    public Vector3 spawnPosition;
    public bool jumping = false;


    // Start is called before the first frame update
    void Awake()
    {
        gameData = gameObject.AddComponent<GameData>();
        rb2d = GetComponent<Rigidbody2D>();
        gina = gameObject.GetComponent<SpriteRenderer>();
        shield = transform.Find("tempShield").gameObject.GetComponent<SpriteRenderer>();
        shield.enabled = false;

        shieldIcon = transform.Find("Shield").gameObject.GetComponent<SpriteRenderer>();
        shieldIcon.enabled = false;
        gloveIcon = transform.Find("BoxingGlove").gameObject.GetComponent<SpriteRenderer>();
        gloveIcon.enabled = false;
        featherIcon = transform.Find("Feather").gameObject.GetComponent<SpriteRenderer>();
        featherIcon.enabled = false;

        key1Icon = transform.Find("Key1").gameObject.GetComponent<SpriteRenderer>();
        key1Icon.enabled = false;
        key2Icon = transform.Find("Key2").gameObject.GetComponent<SpriteRenderer>();
        key2Icon.enabled = false;
        key3Icon = transform.Find("Key3").gameObject.GetComponent<SpriteRenderer>();
        key3Icon.enabled = false;

        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region movement
        float horzMovement = Input.GetAxisRaw("horzAxis")*speed;
        Vector2 movement = new Vector2 (horzMovement, 0);
        
        rb2d.AddForce (movement * speed);
        

        if(Input.GetAxisRaw("horzAxis") == 0){
            rb2d.velocity = new Vector2((rb2d.velocity.x*0.5f),rb2d.velocity.y);
        }

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

        #endregion

        #region Action handling
    
        if(!jumping){
            anim.SetBool("Jumping",false);
        }

        //Jumping Logic
        if(Input.GetButtonDown("aButton"))
        {

            if(!jumping || (playerCanDoubleJump && hasDoubleJump)){
                Vector3 jumpMovement = new Vector3 (0.0f, 1.0f,0.0f);
                rb2d.velocity = jumpMovement * jumpSpeed;

                if(jumping && playerCanDoubleJump){
                    playerCanDoubleJump = false;
                }
                anim.SetBool("Jumping",true);
                jumping = true; 
            }
        }

        //Punch Logic
        if(Input.GetButtonDown("bButton") && !jumping && hasHornPunch && !punching){
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
       punching = true;       
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
        punching = false;
    }
    public void ChecklandedOnFloor(){
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position,Vector2.down);
        if((hit.collider.gameObject.CompareTag("Ground") || (hit.collider.gameObject.CompareTag("enemy"))) && hit.distance < 2f){
            if(hasDoubleJump){
                playerCanDoubleJump = true;
            }
            if(jumping)jumping=false;
        }
    }

    public void playerDeath(){
        StartCoroutine(Flash());
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
            if (n == 0) gameObject.transform.position = spawnPosition;
            yield return new WaitForSecondsRealtime(0.1f);
            gina.enabled = true;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    void OnTriggerEnter2D (Collider2D collision){
        if(collision.gameObject.CompareTag("firstKey")){
            hasFirstKey = true;
            key1Icon.enabled = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("secondKey")){
            hasSecondKey = true;
            key2Icon.enabled = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("thirdKey")){
            hasThirdkey = true;
            key3Icon.enabled = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("doubleJumpItem")){
            hasDoubleJump = true;
            playerCanDoubleJump = true;
            featherIcon.enabled = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("punchItem")){
            hasHornPunch = true;
            gloveIcon.enabled = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("shieldItem")){
            hasShield = true;
            canActivateShield = true;
            shieldIcon.enabled = true;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("enemy") && !shieldIsActive){
            playerDeath();
        }
        if (collision.gameObject.name == "FrontDoor")
        {
            var doorKeys = collision.gameObject.GetComponentsInChildren<SpriteRenderer>();
            if (hasFirstKey)
            {
                doorKeys[1].enabled = true;
            }
            if (hasSecondKey)
            {
                doorKeys[2].enabled = true;
            }
            if (hasThirdkey)
            {
                doorKeys[3].enabled = true;
            }
        }
    }

    //Check if the object is on the ground
    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            ChecklandedOnFloor();
        }
        if(collision.gameObject.CompareTag("enemy"))
        {
            ChecklandedOnFloor();
        }
    }

    //Check if the object has left the ground
    void OnCollisionExit2D (Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            

        }
    }
}
