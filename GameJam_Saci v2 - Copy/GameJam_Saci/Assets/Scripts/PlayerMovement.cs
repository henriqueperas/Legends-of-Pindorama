using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Colocar o rigidbody2D altomaticamente caso o componente que recebera esse script n�o o tenha ainda 
[RequireComponent(typeof(Rigidbody2D))]
//Colocar o Animator altomaticamente caso o componente que recebera esse script n�o o tenha ainda 
[RequireComponent(typeof(Animator))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Configura��es de Movimenta��o")]
    [Tooltip("Velocidade de movimento do jogador.")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        DisableGravity();
    }

    private void Update()
    {
        
        HandleInput();
        
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        
        MovePlayer();
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        
        movement = new Vector2(horizontal, vertical).normalized;
    }

    private void MovePlayer()
    {
        
        rb.velocity = movement * moveSpeed;
    }

    private void UpdateAnimation()
    {
        // Se o jogador estiver parado, ativa a anima��o de idle
        if (movement == Vector2.zero)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Tras", false);
            animator.SetBool("Frente", false);
            animator.SetBool("Lado", false);
        }
        else
        {
            animator.SetBool("Idle", false);

            // Verifica dire��o do movimento e atualiza anima��o
            if (movement.y > 0)
            {
                animator.SetBool("Tras", true); // Anima��o de Tras
                animator.SetBool("Frente", false);
                animator.SetBool("Lado", false);
            }
            else if (movement.y < 0)
            {
                animator.SetBool("Tras", false);
                animator.SetBool("Frente", true); // Anima��o de Frente
                animator.SetBool("Lado", false);
            }
            else
            {
                animator.SetBool("Tras", false);
                animator.SetBool("Frente", false);
                animator.SetBool("Lado", true); // Anima��o de Lado
            }

            // Controle de flip de anima��o
            if (movement.x < 0)
            {
                
                transform.localScale = new Vector3(-3f, 3f, 3f);
            }
            else if (movement.x > 0)
            {
                
                transform.localScale = new Vector3(3f, 3f, 3f);
            }
        }
    }

    private void DisableGravity()
    {
        rb.gravityScale = 0;
    }
}