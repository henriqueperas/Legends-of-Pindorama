using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class BossHealthIara : MonoBehaviour
{
    [Header("Configura��o de Vida")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Configura��o de Dano")]
    public int damageAmount = 15;
    public SpriteRenderer spriteRenderer;

    [Header("Configura��o de Cena")]
    public string cutsceneSceneName = "CutSceneIara"; // Nome da cena da cutscene

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

        // Destr�i o Boss e muda para a cena da cutscene
        Destroy(gameObject);
        SceneManager.LoadScene(cutsceneSceneName);
    }
}