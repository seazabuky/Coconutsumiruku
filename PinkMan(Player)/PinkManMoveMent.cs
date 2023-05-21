using System.Xml.Serialization;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PinkManMoveMent : MonoBehaviour
{
    
    // Start is called before the first frame update
    public Animator animator;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public Transform feetPos;
    public float checkRadius;
    public float runSpeed;
    public float jumpForce;
    public float jumpTime;
    public float jumpCooldown;
    
    private int points = 0;
    private int jumpCount;
    private float lastJumpTime;
    
    float horizontalMove = 0f;
    float jumpTimeCounter;
    float timeSinceJump;

    float wallSlideSpeed = 1f;
    float wallJumpTime = 0.2f;
    bool isWallSliding;
    bool canWallJump;

    bool isTouchingWall;
    bool isFalling;
    bool isWalls;
    bool isJumping;
    bool isGrounded;
    bool doubleJumpAvailable;

    float currentTime;
    public int startMinutes;
    public Text currentTimeText;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector2 wallJumpDirection;
    public GameObject m1;
    [SerializeField] private Text ShowPoint;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource itemCollectSound;

    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        PlayerPrefs.SetString("timerActive", "true");
        currentTime = startMinutes * 60;
    }
    void Update()
    {
        if(!PauesMenu.isPaused)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            Jump();
            Flip();
            if(Convert.ToBoolean(PlayerPrefs.GetString("timerActive"))){
                currentTime = currentTime + Time.deltaTime;
            }
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            string timer = time.ToString(@"hh\:mm\:ss");//time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
            currentTimeText.text ="Timer: " + timer;
            PlayerPrefs.SetString("time", timer);
        }
        
    }
    void FixedUpdate(){
        rb.velocity = new Vector2(horizontalMove * runSpeed * Time.deltaTime, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }
    public void onLanding(){
        animator.SetBool("IsJumping", false);
		animator.SetBool("IsFalling", false);
        animator.SetBool("IsWallJumping", false);
        animator.SetBool("IsDoubleJumping", false);
    }

    void Jump(){
        // Check if the player is grounded set idle animation and reset jump count and double jump availability
        // If the player is not grounded, check if the player is falling and set falling animation
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        isTouchingWall = Physics2D.OverlapCircle(transform.position, 0.2f, whatIsWall);

        // ****************************** Wall Jump & Slide ****************************** //
        if(isTouchingWall && rb.velocity.y < 0){
            animator.SetBool("IsWallJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            isWallSliding = true;
        }else{
            animator.SetBool("IsWallJumping", false);
            isWallSliding = false;
        }
        if(canWallJump && Input.GetButtonDown("Jump")){
            rb.velocity = new Vector2(jumpForce * wallJumpDirection.x, jumpForce);
            canWallJump = false;
            Invoke("ResetCanWallJump", wallJumpTime);
        }

        // ****************************** IDLE % FALLING ****************************** //
        if(isGrounded){
            animator.SetBool("IsJumping", false);
			animator.SetBool("IsFalling", false);
            animator.SetBool("IsWallJumping", false);
            animator.SetBool("IsDoubleJumping", false);
            jumpCount = 0;
            doubleJumpAvailable = true;
            
        }else if(rb.velocity.y < 0){
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
        }
        // ****************************** Jump ****************************** //
        // If the player is grounded and the jump button is pressed, set the jump animation and add force to the player
        // If the player is not grounded, check if the player is falling and set falling animation
        if(isGrounded && Input.GetButtonDown("Jump")){
            jumpSound.Play();
            animator.SetBool("IsJumping", true);
            rb.velocity = Vector2.up * jumpForce;
            jumpCount++;
            lastJumpTime = Time.time;
        }
        // ****************************** Double Jump ****************************** //
        // If the player is not grounded and the double jump button is pressed, set the jump animation and add force to the player
        // If the player is not grounded, check if the player is falling and set falling animation
        else if(!isGrounded && Input.GetButtonDown("Jump") && doubleJumpAvailable){
            jumpSound.Play();
            animator.SetBool("IsDoubleJumping", true);
            rb.velocity = Vector2.up * jumpForce;
            jumpCount++;
            doubleJumpAvailable = false;
        }

        // else if(!isGrounded && Input.GetButton("Jump") && jumpCount < jumpTime && Time.time < lastJumpTime + jumpCooldown){
        //     rb.velocity = Vector2.up * jumpForce;
        //     jumpCount++;
        // }
        // ****************************** Falling after jump ****************************** //
        // If the player is not grounded and the jump button is released, set the falling animation
        if(!isGrounded && Input.GetButtonUp("Jump")){
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
        }

        
    }
    void Flip(){
        if(horizontalMove < 0){
            sr.flipX = true;
        }else if(horizontalMove > 0){
            sr.flipX = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Points")){
            itemCollectSound.Play();
            Destroy(collision.gameObject);
            points++;
            PlayerPrefs.SetInt("Points", points);
            ShowPoint.text = "Points: " + points;
            if(points==1 && PlayerPrefs.GetInt("lastStage") <= SceneManager.GetActiveScene().buildIndex){
                m1.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "enemy"){
            PlayerPrefs.SetInt("Points", 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(collision.gameObject.layer == whatIsWall) {
            wallJumpDirection = new Vector2(collision.contacts[0].normal.x, collision.contacts[0].normal.y);
            if (isWallSliding)
            {
                canWallJump = true;
            }
        }
    }
    void ResetCanWallJump()
    {
        animator.SetBool("IsWallJumping",false);
        canWallJump = false;
    }

}
