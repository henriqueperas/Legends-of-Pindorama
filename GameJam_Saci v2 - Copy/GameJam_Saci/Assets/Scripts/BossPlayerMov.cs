using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayerMov : MonoBehaviour
{
    [Header("Movimentação")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    private float horizontalInput;

    [Header("Checagem de chão")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing = false;
    private float dashTime = 0f;
    private Vector2 dashDirection;

    private Animator anim;

    [Header("Efeitos Sonoros")]
    public AudioManager audioManager; // Referência ao AudioManager
    public AudioClip walkSound; // Som de andar
    public AudioClip jumpSound; // Som de pulo
    public AudioClip dashSound; // Som de dash

    private bool isWalking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (audioManager == null) audioManager = FindObjectOfType<AudioManager>(); // Tenta encontrar o AudioManager se não estiver atribuído
    }

    void Update()
    {
        // Entrada horizontal
        horizontalInput = Input.GetAxis("Horizontal");

        // Pulo
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
                canDoubleJump = true;
                audioManager.PlaySoundEffect(jumpSound); // Toca o som de pulo com controle de volume
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
                audioManager.PlaySoundEffect(jumpSound); // Toca o som de pulo com controle de volume
            }
        }

        // Dash
        if (!isDashing && Input.GetMouseButton(1))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartDash(Vector2.right);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                StartDash(Vector2.left);
            }
        }

        // Checagem de chão
        CheckGround();

        // Atualiza animações
        anim.SetBool("JumpSaci", !isGrounded);
        anim.SetBool("IdleSaci", isGrounded && horizontalInput == 0);
        anim.SetBool("RunSaci", isGrounded && horizontalInput != 0);

        // Tocar o som de andar se o player estiver andando
        if (isGrounded && horizontalInput != 0 && !isWalking)
        {
            isWalking = true;
            audioManager.PlaySoundEffect(walkSound); // Toca o som de andar com controle de volume
        }
        else if (isGrounded && horizontalInput == 0 && isWalking)
        {
            isWalking = false;
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = new Vector2(dashDirection.x * dashSpeed, rb.velocity.y);
            dashTime -= Time.fixedDeltaTime;
            if (dashTime <= 0f)
            {
                isDashing = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void StartDash(Vector2 direction)
    {
        isDashing = true;
        dashTime = dashDuration;
        dashDirection = direction;
        anim.SetTrigger("DestTornado");
        audioManager.PlaySoundEffect(dashSound); // Toca o som de dash com controle de volume
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}