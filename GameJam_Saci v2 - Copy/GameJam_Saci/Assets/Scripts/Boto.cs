using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boto : MonoBehaviour
{
    public float velocidade = 10f;
    public int maxRicochetes = 5;
    public LayerMask camadasParaRicochete;

    private int ricochetes = 0;
    private Rigidbody2D rb;
    private Vector2 direcao;
    private bool seguindoPlayer = true;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Define um lado aleatório para spawn (esquerda ou direita)
        float lado = Random.value < 0.5f ? -1f : 1f;
        transform.position = new Vector2(lado * Camera.main.orthographicSize * 1.2f, Random.Range(-2f, 2f));

        StartCoroutine(PrepararTiro());
    }

    IEnumerator PrepararTiro()
    {
        yield return new WaitForSeconds(3f); // Espera 3 segundos mirando no player

        if (player != null)
        {
            direcao = (player.position - transform.position).normalized;
        }
        seguindoPlayer = false;
        rb.velocity = direcao * velocidade;
    }

    void Update()
    {
        if (seguindoPlayer && player != null)
        {
            // Continua mirando no player até o tempo acabar
            direcao = (player.position - transform.position).normalized;
        }

        if(player.position.x <= gameObject.transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Ground"))
        {
            if (ricochetes < maxRicochetes)
            {
                // Calcula o novo vetor de direção após ricochete
                direcao = Vector2.Reflect(direcao, colisao.contacts[0].normal);
                rb.velocity = direcao * velocidade;
                ricochetes++;
            }
            else
            {
                // Ignora colisões após 5 ricochetes
                GetComponent<Collider2D>().enabled = false;
            }
        }

        // Caso o míssil colida com o jogador ou outro objeto
        if (colisao.gameObject.CompareTag("Player"))
        {
            Debug.Log("Míssil atingiu o jogador!");
            // Aqui você pode aplicar dano ou efeitos

            Destroy(gameObject); // Destroi o míssil após a colisão
        }
        
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}