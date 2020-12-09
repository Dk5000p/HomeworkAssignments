using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    public float moveInput;
    private Rigidbody rb;
    private bool facingRight = true;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public AudioClip stomp;
    private int extraJumps;
    public int extraJumpsValue;
    public int health;
    public Animator anim;

    private void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(moveInput * speed, rb.velocity.y,rb.velocity.z);
        if (facingRight == false && moveInput > 0)
        {
            Flip();
            //anim.SetBool("isWalking", true);
        }
        if (facingRight == true && moveInput < 0)
        {
            Flip();
            //anim.SetBool("isWalking", true);
        }
        if (moveInput == 0)
        {
            //anim.SetBool("isWalking", false);
        }

    }
    private void Update()
    {
        if (isGrounded == true)
        {
            extraJumps = 2;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0|| Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector3.up * jumpForce;
            extraJumps--;
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}

