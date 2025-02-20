using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveMap : MonoBehaviour
{
    public MainMap mapa;

    public Transform[] pontos; // Lista de pontos de movimentação
    public float velocidade = 5f;
    private int indiceAtual = 0;
    private bool movendo = false;
    bool podeLutar = false;

    void Update()
    {
        if (!movendo)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Mover(-1); // Move para cima (posição anterior na lista)
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                Mover(1); // Move para baixo (posição seguinte na lista)
            }
            else if (Input.GetKeyDown(KeyCode.Return) && podeLutar == true) // Enter para entrar na cena do boss
            {
                EntrarNaLuta();
            }
        }
    }

    void Mover(int direcao)
    {
        int novoIndice = indiceAtual + direcao;

        if (novoIndice >= 0 && novoIndice < pontos.Length)
        {
            indiceAtual = novoIndice;
            StartCoroutine(MoverAtePonto(pontos[indiceAtual].position));
        }
    }

    IEnumerator MoverAtePonto(Vector3 destino)
    {
        movendo = true;

        while ((transform.position - destino).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidade * Time.deltaTime);
            yield return null;
        }

        movendo = false;
    }

    void EntrarNaLuta()
    {
        if(mapa.level == 1)
        {
            Debug.Log("aaaaaaaaaaaaa");
            SceneManager.LoadScene("CorpoSeco");
        }
        else if (mapa.level == 2)
        {
            SceneManager.LoadScene("Cuca");
        }
        else if (mapa.level == 3)
        {
            SceneManager.LoadScene("Iara");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            podeLutar = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            podeLutar = false;
        }
    }
}
