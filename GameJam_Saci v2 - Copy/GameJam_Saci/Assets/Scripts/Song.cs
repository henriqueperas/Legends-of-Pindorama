using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    private GameObject target;
    private Vector3 direction;

    public void SetTarget(GameObject target)
    {
        this.target = target;
        // Mover em direção ao alvo
        direction = (target.transform.position - transform.position).normalized;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position += direction * speed * Time.deltaTime;

        // Checar colisão
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            HitTarget();
        }

        if (transform.position.y <= -6)
        {
            Destroy(gameObject);
        }
    }

    void HitTarget()
    {
        PlayerHealth playerH = target.GetComponent<PlayerHealth>();
        if (playerH != null)
        {
            playerH.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}