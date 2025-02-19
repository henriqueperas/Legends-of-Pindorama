using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrocaDeCena : MonoBehaviour
{
    public string scenas;
    private Animator animator;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();

        button.onClick.AddListener(() => StartCoroutine(ExecutarTrocaDeCena()));
    }

    private IEnumerator ExecutarTrocaDeCena()
    {
        button.interactable = false; 
        Time.timeScale = 1;

        if (animator != null)
        {
            animator.SetTrigger("Click"); 
        }

        yield return new WaitForSeconds(3f); 

        SceneManager.LoadScene(scenas);
    }
}