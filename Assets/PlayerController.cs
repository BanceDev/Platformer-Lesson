using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 10f;
    private float moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;

    public Transform groundCheck;
    public float checkRadius = 0.5f;
    public LayerMask ground;
    private int extraJumps = 1;
    public float jumpForce = 15f;
    private Animator anim;
    private bool isGrounded => Physics2D.OverlapCircle(groundCheck.position, checkRadius, ground);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            extraJumps = 1;
        }

        if (Input.GetKeyDown("up") && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown("up") && extraJumps == 0 && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if (rb.velocity.y > 0 && !isGrounded)
        {
            anim.SetBool("Jumping", true);
        }
        else if (rb.velocity.y < 0 && !isGrounded)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", true);
        }
        else if (isGrounded)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", false);
        }
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        if (facingRight && moveInput < 0)
        {
            Flip();
        }
        else if (!facingRight && moveInput > 0)
        {
            Flip();
        }

        if (moveInput != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
