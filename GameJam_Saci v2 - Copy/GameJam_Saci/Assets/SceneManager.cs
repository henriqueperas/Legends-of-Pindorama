using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public MainMap mapa;
    public Sprite bossWin;

    public Animator anim;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("aaaaaaaaaaaa");

            if (mapa.level == 1)
            {
                mapa.level++;
                SceneManager.LoadScene("CorpoSeco");
                anim.SetBool("CucaDesbloqueada", true);
            }
            else if (mapa.level == 2)
            {
                mapa.level++;
                SceneManager.LoadScene("Cuca");
                anim.SetBool("IaraDesbloqueada", true);
            }
            else if (mapa.level == 3)
            {
                mapa.level++;
                SceneManager.LoadScene("Iara");
            }

        }
    }
}