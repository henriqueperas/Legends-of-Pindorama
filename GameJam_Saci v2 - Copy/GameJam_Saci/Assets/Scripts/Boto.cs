using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boto : MonoBehaviour
{
    public Transform player;           // Referência ao jogador
    public float speed = 5f;           // Velocidade inicial do míssil
    public float acceleration = 0.5f;  // Aceleração do míssil
    public float rotationSpeed = 200f; // Velocidade de rotação do míssil
    public float chaseDuration = 10f; // Duração da perseguição (5 minutos em segundos)

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

            // Calcular direção para o jogador
            Vector2 direction = (Vector2)player.position - rb.position;
            direction.Normalize();

            // Ajustar a direção gradualmente para criar o efeito de derrapagem
            float rotateAmount = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = -rotateAmount * rotationSpeed;

            // Aplicar velocidade e aceleração
            rb.velocity = transform.right * speed;
            speed += acceleration * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject); // Destroi o míssil após o tempo de perseguição
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Caso o míssil colida com o jogador ou outro objeto
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Míssil atingiu o jogador!");
            // Aqui você pode aplicar dano ou efeitos
        }
        Destroy(gameObject); // Destroi o míssil após a colisão
    }
}