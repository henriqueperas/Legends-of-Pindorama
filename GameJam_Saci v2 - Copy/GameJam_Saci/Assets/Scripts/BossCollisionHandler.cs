using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollisionHandler : MonoBehaviour
{

    public Camera mainCamera; // A câmera principal
    public Camera[] bossCameras; // Array com as câmeras dos bosses
    public MonoBehaviour playerMovementScript; // O script de movimento do player

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a tag é de algum boss
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

                        // Troca para a câmera do boss correspondente
                        SwitchToBossCamera(i);
                    }
                    else
                    {
                        Debug.Log($"O {bossTag} ainda está bloqueado!");
                    }
                }
                else
                {
                    Debug.LogWarning($"O objeto com a tag {bossTag} não possui o componente BossStatus.");
                }
                return; // Sai do loop após encontrar o boss correspondente
            }
        }
    }

    private void SwitchToBossCamera(int bossIndex)
    {
        // Desativa todas as câmeras
        mainCamera.enabled = false;
        foreach (Camera cam in bossCameras)
        {
            cam.enabled = false;
        }

        // Ativa a câmera do boss correspondente
        if (bossIndex >= 0 && bossIndex < bossCameras.Length)
        {
            bossCameras[bossIndex].enabled = true;
        }
        else
        {
            Debug.LogError("Índice de câmera inválido.");
        }
    }
}

// Classe para gerenciar o status do boss
public class BossStatus : MonoBehaviour
{
    public bool desbloqueado = false; // Define se o boss está desbloqueado
}