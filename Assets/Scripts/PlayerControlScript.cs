using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControlScript : MonoBehaviour
{
    //TEST
    public float moveSpeed;
    private float moveInput;
    public float jumpForce;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;
    public float jumpTime;
    public Transform feetPos;
    public float circleRadius;
    public LayerMask whatIsground;

    //Declarations
    public Rigidbody2D rb2d;
    public CapsuleCollider2D ginaCollider;
    public AudioClip spoopy2;
    public AudioSource audioSource;

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
    private int shieldCoolDown = 3;
    public Animator anim;
    public GameData gameData;
    public bool facingRight = true;
    public Vector3 spawnPosition;
    public bool jumping = false;


    // Start is called before the first frame update
    void Awake()
    {
        moveSpeed = 5;
        jumpForce = 8;
        circleRadius = .3f;
        isJumping = false;
        jumpTime = .25f;



        gameData = gameObject.AddComponent<GameData>();
        rb2d = GetComponent<Rigidbody2D>();
        ginaCollider = GetComponent<CapsuleCollider2D>();
        gina = gameObject.GetComponent<SpriteRenderer>();
        shield = transform.Find("tempShield").gameObject.GetComponent<SpriteRenderer>();
        shield.enabled = false;

        var camera = transform.parent.Find("CharCamera").transform;
        shieldIcon = camera.Find("Shield").gameObject.GetComponent<SpriteRenderer>();
        shieldIcon.enabled = false;
        gloveIcon = camera.Find("BoxingGlove").gameObject.GetComponent<SpriteRenderer>();
        gloveIcon.enabled = false;
        featherIcon = camera.Find("Feather").gameObject.GetComponent<SpriteRenderer>();
        featherIcon.enabled = false;

        key1Icon = camera.Find("Key1").gameObject.GetComponent<SpriteRenderer>();
        key1Icon.enabled = false;
        key2Icon = camera.Find("Key2").gameObject.GetComponent<SpriteRenderer>();
        key2Icon.enabled = false;
        key3Icon = camera.Find("Key3").gameObject.GetComponent<SpriteRenderer>();
        key3Icon.enabled = false;

        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("horzAxis");
        rb2d.velocity = new Vector2(moveInput * moveSpeed * (isGrounded ? 1 : 1.5f), rb2d.velocity.y);
        if (moveInput > 0)
        {
            facingRight = true;
            gina.flipX = false;
            anim.SetTrigger("GinaWalk");
        } else if (moveInput < 0)
        {
            facingRight = false;
            gina.flipX = true;
            anim.SetTrigger("GinaWalk");
        } else
        {
            anim.SetTrigger("GinaIdle");
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, circleRadius, whatIsground);
        anim.SetBool("Jumping", !isGrounded);
        if (isGrounded)
        {
            playerCanDoubleJump = true;
        }

        if ((isGrounded || (playerCanDoubleJump && hasDoubleJump)) && Input.GetButtonDown("aButton"))
        {
            if (!isGrounded && !isJumping)
            {
                playerCanDoubleJump = false;
            }
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb2d.velocity = Vector2.up * jumpForce;
        }

        if (isJumping && Input.GetButton("aButton"))
        {
            if (jumpTimeCounter > 0)
            {
                rb2d.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }
        }
        if (Input.GetButtonUp("aButton"))
        {
            isJumping = false;
        }

        if (Input.GetButtonDown("bButton") && !jumping && hasHornPunch && !punching)
        {
            Punch();
            anim.SetTrigger("GinaPunch");
        }

        //Shield Logic
        if (Input.GetButtonDown("xButton") && hasShield)
        {
            if (canActivateShield)
            {
                StartCoroutine(ActivateShield());
            }
        }
    }

   public void Punch(){ 
       punching = true;       
        RaycastHit2D hit;
        if(facingRight)
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
        foreach(var enemy in GameObject.FindGameObjectsWithTag("enemy")) {
            if (ginaCollider.IsTouching(enemy.GetComponent<BoxCollider2D>())) {
                playerDeath();
            }

        }
        yield return new WaitForSecondsRealtime(shieldCoolDown);
        canActivateShield = true;
    }

    IEnumerator Flash()
    {
        rb2d.velocity = new Vector2(0,0);
        for (int n = 0; n < 5; n++)
        {
            gina.enabled=false;
            if (n == 0) gameObject.transform.position = spawnPosition;
            yield return new WaitForSecondsRealtime(0.1f);
            gina.enabled = true;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    IEnumerator ChangeSong()
    {
        var tte = audioSource.clip.length - audioSource.time;
        if (tte > 0)
        {
            yield return new WaitForSecondsRealtime(tte);
        }
        audioSource.clip = spoopy2;
        audioSource.Play();
        yield return null;
        
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
            StartCoroutine(ChangeSong());
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
                SceneManager.LoadScene("EndScene");
            }
        }
    }
}
