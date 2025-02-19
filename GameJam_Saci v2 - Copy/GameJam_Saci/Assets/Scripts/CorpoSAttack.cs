using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpoSAttack : MonoBehaviour
{
    [Header("Configuração de Ataque")]
    public GameObject projectilePrefab;  // Prefab do projétil
    public GameObject secondAttackPrefab; // Prefab do segundo ataque (fase 2)
    public Transform player; // Referência ao jogador
    public float projectileSpeed = 5f; // Velocidade do projétil
    public float secondAttackSpeed = 3f; // Velocidade do segundo ataque

    [Header("Configuração de Rotação")]
    public float rotationSpeedAtaqueOssos = 180f; // Velocidade de rotação do AtaqueOssos (graus por segundo)
    public float rotationSpeedAtaqueCranio = 180f; // Velocidade de rotação do AtaqueCranio (graus por segundo)

    public bool isSecondAttackActivated = false; // Flag para verificar se o segundo ataque foi ativado
    private bool isUsingFirstAttack = true; // Flag para determinar qual ataque usar

    [Header("Configuração de Animação")]
    public Animator animator; // Referência ao Animator do corpo seco

    [Header("Posições de Spawn")]
    public Transform positionATKO; // Posição de spawn para o AtaqueOssos
    public Transform positionATKC; // Posição de spawn para o AtaqueCranio

    [Header("Efeitos Sonoros")]
    public AudioManager audioManager; // Referência ao AudioManager
    public AudioClip arremesoSound; // Som do primeiro ataque (arremesso)
    public AudioClip ossosQuebrandoSound; // Som do segundo ataque (ossos quebrando)

    void Start()
    {
        StartCoroutine(AttackLoop());

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>(); // Tenta encontrar o AudioManager se não estiver atribuído
        }
    }

    public void ActivateSecondAttack()
    {
        isSecondAttackActivated = true;
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            if (isUsingFirstAttack)
            {
                StartCoroutine(AttackPhaseOne());
            }
            else
            {
                StartCoroutine(AttackPhaseTwo());
            }

            isUsingFirstAttack = !isUsingFirstAttack;

            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator AttackPhaseOne()
    {
        animator.SetTrigger("AtaqueOssos");

        if (player != null && positionATKO != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, positionATKO.position, Quaternion.identity);
            Vector2 direction = (player.position - positionATKO.position).normalized;
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * projectileSpeed;

            StartCoroutine(RotateAttack(projectile, rotationSpeedAtaqueOssos));
            StartCoroutine(DestroyProjectileOnPositionX(projectile, -15.74f));

            Destroy(projectile, 20f);
        }

        yield return null;
    }

    private IEnumerator AttackPhaseTwo()
    {
        animator.SetTrigger("AtaqueCranio");

        if (player == null || positionATKC == null) yield break;

        GameObject secondAttack = Instantiate(secondAttackPrefab, positionATKC.position, Quaternion.identity);
        Rigidbody2D rb = secondAttack.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.position - positionATKC.position).normalized;

        StartCoroutine(MoveAndRotateAttack(secondAttack, rb, direction));
        StartCoroutine(DestroyProjectileOnPositionX(secondAttack, -15.74f));

        Destroy(secondAttack, 20f);

        // Toca o som de ossos quebrando (segundo ataque)
        audioManager.PlaySoundEffect(ossosQuebrandoSound);

        yield return null;
    }

    private IEnumerator MoveAndRotateAttack(GameObject attack, Rigidbody2D rb, Vector2 direction)
    {
        while (attack != null)
        {
            rb.velocity = direction * secondAttackSpeed;
            attack.transform.Rotate(0, 0, rotationSpeedAtaqueCranio * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator RotateAttack(GameObject attack, float rotationSpeed)
    {
        while (attack != null)
        {
            attack.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator DestroyProjectileOnPositionX(GameObject projectile, float targetX)
    {
        while (projectile != null)
        {
            if (projectile.transform.position.x <= targetX)
            {
                Destroy(projectile);
                yield break;
            }
            yield return null;
        }
    }

    public void TakeDamage(int damage)
    {
    }

    public void UpdateAnimation(bool isIdle)
    {
        if (isIdle)
        {
            animator.SetBool("Idle", true);
        }
        else
        {
            animator.SetBool("Idle", false);
        }
    }
}