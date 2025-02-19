using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKPlayer : MonoBehaviour
{
    public float attackDistance = 5f; // Distância máxima que o bumerangue irá atingir
    public float speed = 5f; // Velocidade do ataque
    public GameObject projectilePrefab; // O prefab do ataque (pode ser uma imagem ou sprite)
    public Transform attackPosition; // Posição controlada pelo Empty (Transform)
    private Animator animator; // Componente Animator do jogador

    private bool isAttacking = false; // Flag para verificar se o ataque foi lançado

    void Start()
    {
        // Obtém o componente Animator no início
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verifica se o jogador apertou o botão esquerdo do mouse
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        // Marca que o ataque foi lançado
        isAttacking = true;

        // Aciona a animação de ataque (primeiro para garantir que a animação comece)
        animator.SetBool("ATKSaci", true);
        animator.SetBool("IdleSaci", false);

        // Espera um pouco para garantir que a animação inicie antes de lançar o ataque
        yield return new WaitForSeconds(0.3f);  // Espera o tempo suficiente para a animação

        animator.SetBool("IdleSaci", true);
        animator.SetBool("ATKSaci", false);

        // Cria o objeto do ataque a partir do prefab na posição controlada pelo Empty
        GameObject projectile = Instantiate(projectilePrefab, attackPosition.position, Quaternion.identity);

        // Calcula a direção do ataque (para a direita por exemplo)
        Vector3 targetPosition = attackPosition.position + transform.right * attackDistance;
        float distanceTravelled = 0f;

        // Move o ataque até a posição alvo
        while (distanceTravelled < attackDistance)
        {
            float step = speed * Time.deltaTime;
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, targetPosition, step);
            distanceTravelled += step;
            yield return null;
        }

        // Agora faz o ataque voltar para a posição inicial
        while (distanceTravelled > 0f)
        {
            float step = speed * Time.deltaTime;
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, attackPosition.position, step);
            distanceTravelled -= step;
            yield return null;
        }

        // Após o ataque voltar, destrói o objeto
        Destroy(projectile);
        
        // Marca que o ataque terminou
        isAttacking = false;
    }
}