using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public SpriteRenderer bubble;

    public float speed = 5f;            // Velocidade horizontal
    public float waveAmplitude = 2f;   // Amplitude do movimento em onda (altura)
    public float waveFrequency = 2f;   // Frequência do movimento em onda (velocidade da oscilação)4

    public float timer = 0f;
    private float maxTime = 7f;

    private float startY;               // Posição inicial no eixo Y
    private Vector2 direction;          // Direção do movimento
    private int directionRandom;

    void Start()
    {
        timer = maxTime;
        // Salvar a posição inicial no eixo Y
        startY = transform.position.y;

        speed = Random.Range(1f, 2.25f);

        waveFrequency = Random.Range(1f, 3f);
        waveAmplitude = Random.Range(0.75f, 2.5f);
        directionRandom = Random.Range(0, 2);

        // Definir a direção inicial (da esquerda para a direita ou vice-versa)
        if (directionRandom == 0)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
            bubble.flipX = true;
        }
        
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            Destroy(gameObject);
        }

        // Movimento horizontal
        transform.Translate(direction * speed * Time.deltaTime);

        // Movimento vertical (em onda)
        float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        transform.position = new Vector2(transform.position.x, startY + waveOffset);

        // Confinar o inimigo dentro dos limites da tela
        RestrictMovementToScreenBounds();
    }

    void RestrictMovementToScreenBounds()
    {
        // Confinar o movimento aos limites da tela
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (screenPosition.x > 1f || screenPosition.x < 0f)
        {

            if (bubble.flipX == true)
            {
                bubble.flipX = false;
            }
            else
            {
                bubble.flipX = true;
            }

            // Inverter a direção horizontal quando atingir o limite da tela
            direction = -direction;

            // Ajustar a posição para garantir que fique dentro da tela
            float clampedX = Mathf.Clamp(screenPosition.x, 0.01f, 0.99f);
            transform.position = Camera.main.ViewportToWorldPoint(new Vector2(clampedX, screenPosition.y));
        }
    }
}
