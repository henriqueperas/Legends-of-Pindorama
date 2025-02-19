using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FecharJogo : MonoBehaviour
{
    [SerializeField] private Animator buttonAnimator; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(SairDoJogoComDelay());
        }
    }

    public void SairDoJogo()
    {
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }

    public void BotaoSair()
    {
        Debug.Log("Botão de sair pressionado");
        StartCoroutine(SairDoJogoComDelay());
    }

    private IEnumerator SairDoJogoComDelay()
    {
        if (buttonAnimator != null)
        {
            Debug.Log("Ativando animação...");
            buttonAnimator.SetTrigger("Click"); 
        }
        else
        {
            Debug.LogError("Animator não foi atribuído! Arraste o Animator do botão no Inspector.");
        }

        yield return new WaitForSeconds(3f); 

        SairDoJogo();
    }
}