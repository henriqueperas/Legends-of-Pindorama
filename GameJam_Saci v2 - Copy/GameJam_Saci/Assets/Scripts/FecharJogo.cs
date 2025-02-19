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
        Debug.Log("Bot�o de sair pressionado");
        StartCoroutine(SairDoJogoComDelay());
    }

    private IEnumerator SairDoJogoComDelay()
    {
        if (buttonAnimator != null)
        {
            Debug.Log("Ativando anima��o...");
            buttonAnimator.SetTrigger("Click"); 
        }
        else
        {
            Debug.LogError("Animator n�o foi atribu�do! Arraste o Animator do bot�o no Inspector.");
        }

        yield return new WaitForSeconds(3f); 

        SairDoJogo();
    }
}