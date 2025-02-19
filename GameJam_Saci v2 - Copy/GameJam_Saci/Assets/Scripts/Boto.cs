using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boto : MonoBehaviour
{
    public Transform player;           // Refer�ncia ao jogador
    public float speed = 5f;           // Velocidade inicial do m�ssil
    public float acceleration = 0.5f;  // Acelera��o do m�ssil
    public float rotationSpeed = 200f; // Velocidade de rota��o do m�ssil
    public float chaseDuration = 10f; // Dura��o da persegui��o (5 minutos em segundos)

    private Rigidbody2D rb;
    private float chaseTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        chaseTimer = chaseDuration;
    }

    void Update()
    {
        if (chaseTimer > 0)
        {
            chaseTimer -= Time.deltaTime;

            // Calcular dire��o para o jogador
            Vector2 direction = (Vector2)player.position - rb.position;
            direction.Normalize();

            // Ajustar a dire��o gradualmente para criar o efeito de derrapagem
            float rotateAmount = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = -rotateAmount * rotationSpeed;

            // Aplicar velocidade e acelera��o
            rb.velocity = transform.right * speed;
            speed += acceleration * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject); // Destroi o m�ssil ap�s o tempo de persegui��o
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Caso o m�ssil colida com o jogador ou outro objeto
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("M�ssil atingiu o jogador!");
            // Aqui voc� pode aplicar dano ou efeitos
        }
        Destroy(gameObject); // Destroi o m�ssil ap�s a colis�o
    }
}