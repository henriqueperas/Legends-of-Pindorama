using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKPlayer : MonoBehaviour
{
    public float attackDistance = 5f; // Dist�ncia m�xima que o bumerangue ir� atingir
    public float speed = 5f; // Velocidade do ataque
    public GameObject projectilePrefab; // O prefab do ataque (pode ser uma imagem ou sprite)
    public Transform attackPosition; // Posi��o controlada pelo Empty (Transform)
    private Animator animator; // Componente Animator do jogador

    private bool isAttacking = false; // Flag para verificar se o ataque foi lan�ado

    void Start()
    {
        // Obt�m o componente Animator no in�cio
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verifica se o jogador apertou o bot�o esquerdo do mouse
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        // Marca que o ataque foi lan�ado
        isAttacking = true;

        // Aciona a anima��o de ataque (primeiro para garantir que a anima��o comece)
        animator.SetBool("ATKSaci", true);
        animator.SetBool("IdleSaci", false);

        // Espera um pouco para garantir que a anima��o inicie antes de lan�ar o ataque
        yield return new WaitForSeconds(0.3f);  // Espera o tempo suficiente para a anima��o

        animator.SetBool("IdleSaci", true);
        animator.SetBool("ATKSaci", false);

        // Cria o objeto do ataque a partir do prefab na posi��o controlada pelo Empty
        GameObject projectile = Instantiate(projectilePrefab, attackPosition.position, Quaternion.identity);

        // Calcula a dire��o do ataque (para a direita por exemplo)
        Vector3 targetPosition = attackPosition.position + transform.right * attackDistance;
        float distanceTravelled = 0f;

        // Move o ataque at� a posi��o alvo
        while (distanceTravelled < attackDistance)
        {
            float step = speed * Time.deltaTime;
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, targetPosition, step);
            distanceTravelled += step;
            yield return null;
        }

        // Agora faz o ataque voltar para a posi��o inicial
        while (distanceTravelled > 0f)
        {
            float step = speed * Time.deltaTime;
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, attackPosition.position, step);
            distanceTravelled -= step;
            yield return null;
        }

        // Ap�s o ataque voltar, destr�i o objeto
        Destroy(projectile);
        
        // Marca que o ataque terminou
        isAttacking = false;
    }
}