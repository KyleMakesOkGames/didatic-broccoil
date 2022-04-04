using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform groundCheck;

    public LayerMask groundCheckLayer;

    public float movementSpeed;

    private float moveDirection;

    public float jumpForce;

    private bool isFacingRight;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
        Flip();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundCheckLayer);
    }

    private void HandleInput()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

        if(Input.GetButtonUp("Jump") && rb2d.velocity.y > 0f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }
    }

    private void ApplyMovement()
    {
        rb2d.velocity = new Vector2(moveDirection * movementSpeed, rb2d.velocity.y);
    }

    private void Flip()
    {
        if(isFacingRight && moveDirection < 0 || !isFacingRight && moveDirection > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
