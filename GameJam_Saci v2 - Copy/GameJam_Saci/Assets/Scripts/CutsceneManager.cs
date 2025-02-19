using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Referência ao VideoPlayer na cena
    public string nextSceneName = "Mapa"; // Nome da cena do mapa

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd; // Evento chamado quando o vídeo termina
        }
        else
        {
            Debug.LogError("VideoPlayer não atribuído no inspector!");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName); // Carrega a cena do mapa
    }
}