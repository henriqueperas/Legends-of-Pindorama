using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    [Header("Configuração de Vida")]
    public int maxHealth = 100; // Vida máxima do jogador
    private int currentHealth;  // Vida atual do jogador

    [Header("UI")]
    public Slider healthSlider; // Referência ao slider da UI

    [Header("Configuração de Game Over")]
    public string gameOverSceneName = "GameOver"; // Nome da cena de Game Over
    public float delayBeforeGameOver = 5f; // Tempo de espera antes de mudar para a cena de Game Over

    private bool isDead = false;

    [Header("Configuração do Boss")]
    private BossPlayerMov bossMovementScript; // Referência ao script de movimento do Boss

    void Start()
    {
        // Inicializa a vida no máximo e configura o slider
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        // Obtém o componente BossPlayerMov do próprio GameObject (Player)
        bossMovementScript = GetComponent<BossPlayerMov>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // Verifica a tag do objeto colidido e aplica o dano
        if (collision.CompareTag("AtkC1"))
        {
            TakeDamage(10);
        }
        else if (collision.CompareTag("AtkC2"))
        {
            TakeDamage(20);
        }

        if (collision.gameObject.CompareTag("AtkCuca1"))
        {
            TakeDamage(10);

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("AtkCuca2"))
        {
            TakeDamage(10);
        }
        else if (collision.gameObject.CompareTag("AtkCuca3"))
        {
            TakeDamage(10);
        }



    }

    // Método para tomar dano
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Garante que a vida não passe do limite
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Atualiza o slider na UI
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    // Lógica para a morte do jogador
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player morreu! Desativando movimento do Boss e mudando para a cena de Game Over...");

        // Desativa o movimento do Boss (script BossPlayerMov no próprio player)
        if (bossMovementScript != null)
        {
            bossMovementScript.enabled = false;
        }

        // Inicia o tempo de espera antes do Game Over
        Invoke(nameof(LoadGameOverScene), delayBeforeGameOver);
    }

    // Carrega a cena de Game Over
    private void LoadGameOverScene()
    {
        // Reativa o movimento do Boss antes de mudar para a cena de Game Over
        if (bossMovementScript != null)
        {
            bossMovementScript.enabled = true;
        }

        SceneManager.LoadScene(gameOverSceneName);
    }
}