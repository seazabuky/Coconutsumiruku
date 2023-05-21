using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    public Animator animator;
    public float jumpForce = 10f;  // The force of the jump
    public float wallSlideSpeed = 1f;  // The speed the player slides down the wall
    public LayerMask whatIsWall;  // The layer that represents walls

    private bool isWallSliding = false;  // True if the player is sliding down a wall
    private bool canWallJump = false;  // True if the player can wall jump
    private Rigidbody2D rb;  // The player's rigidbody
    private Vector2 wallJumpDirection;  // The direction of the wall jump
    private float wallJumpTime = 0.2f;  // The length of the wall jump

    SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Check if the player is colliding with a wall
        bool isTouchingWall = Physics2D.OverlapCircle(transform.position, 0.2f, whatIsWall);

        // If the player is touching a wall, slide down it
        if (isTouchingWall && rb.velocity.y < 0)
        {
            animator.SetBool("IsWallJumping",true);
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            isWallSliding = true;        
            //Debug.Log("Wall Sliding");
        }
        else
        {
            animator.SetBool("IsWallJumping",false);
            isWallSliding = false;
            
        }

        // If the player can wall jump and they press the jump button, wall jump
        if (canWallJump && Input.GetButtonDown("Jump"))
        {  
            rb.velocity = new Vector2(jumpForce * wallJumpDirection.x, jumpForce);
            canWallJump = false;
            Invoke("ResetCanWallJump", wallJumpTime);
            //Debug.Log("Wall Jump");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with a wall
        if (collision.gameObject.layer == whatIsWall)
        {
            // Set the direction of the wall jump
            wallJumpDirection = new Vector2(collision.contacts[0].normal.x, collision.contacts[0].normal.y);

            // If the player is sliding down a wall, they can wall jump
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
