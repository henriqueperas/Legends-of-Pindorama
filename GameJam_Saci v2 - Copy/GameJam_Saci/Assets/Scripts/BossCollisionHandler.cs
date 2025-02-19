using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollisionHandler : MonoBehaviour
{

    public Camera mainCamera; // A c�mera principal
    public Camera[] bossCameras; // Array com as c�meras dos bosses
    public MonoBehaviour playerMovementScript; // O script de movimento do player

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a tag � de algum boss
        for (int i = 0; i < bossCameras.Length; i++)
        {
            string bossTag = $"boss{i + 1}"; // Cria a tag dinamicamente (boss1, boss2, ...)
            if (collision.gameObject.CompareTag(bossTag))
            {
                BossStatus bossStatus = collision.gameObject.GetComponent<BossStatus>();

                if (bossStatus != null)
                {
                    if (bossStatus.desbloqueado)
                    {
                        // Desativa o movimento do player
                        playerMovementScript.enabled = false;

                        // Troca para a c�mera do boss correspondente
                        SwitchToBossCamera(i);
                    }
                    else
                    {
                        Debug.Log($"O {bossTag} ainda est� bloqueado!");
                    }
                }
                else
                {
                    Debug.LogWarning($"O objeto com a tag {bossTag} n�o possui o componente BossStatus.");
                }
                return; // Sai do loop ap�s encontrar o boss correspondente
            }
        }
    }

    private void SwitchToBossCamera(int bossIndex)
    {
        // Desativa todas as c�meras
        mainCamera.enabled = false;
        foreach (Camera cam in bossCameras)
        {
            cam.enabled = false;
        }

        // Ativa a c�mera do boss correspondente
        if (bossIndex >= 0 && bossIndex < bossCameras.Length)
        {
            bossCameras[bossIndex].enabled = true;
        }
        else
        {
            Debug.LogError("�ndice de c�mera inv�lido.");
        }
    }
}

// Classe para gerenciar o status do boss
public class BossStatus : MonoBehaviour
{
    public bool desbloqueado = false; // Define se o boss est� desbloqueado
}