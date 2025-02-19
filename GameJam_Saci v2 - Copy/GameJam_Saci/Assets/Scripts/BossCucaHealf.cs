using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class BossCucaHealf : MonoBehaviour
{
    [Header("Configuração de Vida")]
    public int maxHealth = 125;
    private int currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Configuração de Dano")]
    public int damageAmount = 16;
    public SpriteRenderer spriteRenderer;

    [Header("Configuração de Cena")]
    public string cutsceneSceneName = "CutSceneCuca"; // Nome da cena da cutscene

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AtkPlayer"))
        {
            TakeDamage(damageAmount);
        }
    }

    private void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        StartCoroutine(BlinkEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private IEnumerator BlinkEffect()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Boss morreu!");

        // Destrói o Boss e muda para a cena da cutscene
        Destroy(gameObject);
        SceneManager.LoadScene(cutsceneSceneName);
    }
}
