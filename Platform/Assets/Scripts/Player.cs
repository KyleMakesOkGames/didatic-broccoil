using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float jumpingPower;
        [SerializeField] private float coyoteTime;
        [SerializeField] private float jumpBufferTime;

        private bool canDash = true;
        private bool isDashing;
        public float dashingPower;
        public float dashingTime;
        public float dashingCooldown;

        private float horizontal;
        private float coyoteTimeCounter;
        private float jumpBufferCounter;
        private bool isFacingRight = true;

        public LayerMask groundLayer;
        public Transform groundCheck;
        private Rigidbody2D rb;
        private Animator anim;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
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

        private void HandleInput()
        {
            if (isDashing)
                return;

            horizontal = Input.GetAxisRaw("Horizontal");

            if(IsGrounded)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if(Input.GetButtonDown("Jump"))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

                jumpBufferCounter = 0f;
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

                coyoteTimeCounter = 0f;
            }

            if(Input.GetKeyDown(KeyCode.LeftShift) && canDash && horizontal != 0)
            {
                StartCoroutine(Dash());
            }

            if (horizontal != 0 && IsGrounded)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }

        private void ApplyMovement()
        {
            if (isDashing)
                return;
            
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        private void Flip()
        {
            if (isFacingRight && horizontal < 0 || !isFacingRight && horizontal > 0)
            {
                Vector3 localScale = transform.localScale;
                isFacingRight = !isFacingRight;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }

        private IEnumerator Dash()
        {
            canDash = false;
            isDashing = true;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
            yield return new WaitForSeconds(dashingTime);
            rb.gravityScale = originalGravity;
            isDashing = false;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
        }

        private bool IsGrounded => Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
