using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iara : MonoBehaviour
{

    public Animator animArraia;

    [Header("Iara Settings")]
    public int attackIndex;
    public float timeBetweenAttacks = 3f;   // Tempo entre os estados/ataques
    public GameObject player;

    private bool transitioning = false;

    [Header("SongAttack Settings")]
    public float attackCooldown = 1f;
    public int damage = 10;
    public GameObject songPrefab; // Caso use projéteis
    public Transform firePoint; // Ponto de onde o projétil será disparado
    private float attackTimer;

    [Header("SwirlAttack Settings")]
    public GameObject projectilePrefab; // Prefab do objeto que será disparado
    public Transform[] spawnPoints;    // Array de pontos de spawn (3 posições diferentes)
    public float minInterval = 0.5f;   // Intervalo mínimo entre disparos
    public float maxInterval = 2f;     // Intervalo máximo entre disparos
    public float ObstaclesSpeed = 5f; // Velocidade do objeto disparado
    private float nextFireTime;

    [Header("BotoAttack Settings")]
    public GameObject missilePrefab; // Prefab do míssil
    public Transform missileSpawnPoint; // Ponto de origem do míssil
    public float botoDuration = 35f; // Tempo de perseguição

    [Header("StingrayAttack Settings")]
    public GameObject spikePrefab; // Prefab do espinho
    public Transform[] spawnPointsStingray; // 8 pontos onde os espinhos surgirão
    public float warningTime = 2f; // Tempo de aviso antes do ataque
    public float attackSpeed = 10f; // Velocidade de ataque dos espinhos
    public float destroyTime = 1.5f; // Tempo para destruir os espinhos depois do ataque

    private GameObject[] spawnedSpikes = new GameObject[8];

    void Start()
    {

        StartCoroutine(StateMachine());
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (attackIndex)
            {
                case 0:
                    yield return StartCoroutine(IdleState());
                    break;

                case 1:
                    yield return StartCoroutine(SongAttack(player));
                    break;

                case 2:
                    yield return StartCoroutine(SwirlAttack());
                    break;

                case 3:
                    yield return StartCoroutine(BotoAttack());
                    break;

                case 4:
                    yield return StartCoroutine(SpikeAttackRoutine());
                    break;
            }
        }
    }

    private IEnumerator IdleState()
    {
        Debug.Log("Boss está em Idle...");
        yield return new WaitForSeconds(2f); // Tempo de espera no estado Idle
        ChangeState();      // Muda para o primeiro ataque
    }

    private IEnumerator SongAttack(GameObject target)
    {
        int attackController = 0;

        //if (target == null) return;

        while (attackController < 10)
        {
            // Caso use projéteis
            if (songPrefab != null && firePoint != null)
            {
                GameObject song = Instantiate(songPrefab, firePoint.position, Quaternion.identity);
                Song projScript = song.GetComponent<Song>();
                if (projScript != null)
                {
                    projScript.SetTarget(target);
                }
            }
            yield return new WaitForSeconds(0.5f);
            attackController++;
        }
        attackIndex = 0;
    }

    private IEnumerator SwirlAttack()
    {
        int attackController = 0;

        while (attackController < 16)
        {
            ScheduleNextShot();

            // Escolher aleatoriamente um ponto de spawn
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Instanciar o projétil
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

            // Configurar a velocidade do projétil
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (randomIndex < 3)
                rb.velocity = Vector2.right * ObstaclesSpeed; // Movimenta da esquerda para a direita
            else
                rb.velocity = Vector2.left * ObstaclesSpeed; // Movimenta da esquerda para a esqueda

            // Destruir o projétil após 5 segundos para evitar sobrecarga
            Destroy(projectile, 5f);
            yield return new WaitForSeconds(0.5f);
            attackController++;
        }
        attackIndex = 0;
    }

    void ScheduleNextShot()
    {
        minInterval = Mathf.Max(0.1f, minInterval - Time.deltaTime * 0.01f);
        maxInterval = Mathf.Max(0.5f, maxInterval - Time.deltaTime * 0.01f);

        nextFireTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    private IEnumerator BotoAttack()
    {
        GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
        missile.GetComponent<Boto>().player = player.transform;
        yield return new WaitForSeconds(5f);
        attackIndex = 0;
    }

    private IEnumerator SpikeAttackRoutine()
    {
        // Criar os espinhos nos 8 pontos, sem atacar ainda
        for (int i = 0; i < 8; i++)
        {
            Debug.Log(i);
            spawnedSpikes[i] = Instantiate(spikePrefab, spawnPointsStingray[i].position, Quaternion.identity);
        }

        // Espera 2 segundos para o jogador ver onde será atacado
        yield return new WaitForSeconds(warningTime);

        // Ataca os primeiros 4 de forma intercalada
        for (int i = 0; i < 8; i += 2)
        {
            StartCoroutine(AttackSpike(spawnedSpikes[i]));
        }

        yield return new WaitForSeconds(2.2f); // Pequeno delay antes do segundo ataque

        // Escolhe aleatoriamente 4 espinhos para atacar primeiro
        List<int> indices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
        ShuffleList(indices); // Embaralha os índices

        List<int> firstWave = indices.GetRange(0, 4);
        List<int> secondWave = indices.GetRange(4, 4);

        // Ataca os outros 4
        for (int i = 1; i < 8; i += 2)
        {
            StartCoroutine(AttackSpike(spawnedSpikes[i]));
        }


    }

    private IEnumerator AttackSpike(GameObject spike)
    {
        if (spike == null) yield break;

        BoxCollider2D spikeAttack = spike.gameObject.gameObject.GetComponent<BoxCollider2D>();

        spikeAttack.enabled = false;
        animArraia = spike.GetComponent<Animator>();
        animArraia.SetBool("ArraiaAtaca", true);
        /*
        float elapsedTime = 0f;
        Vector3 startPosition = spike.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 4f; // O espinho sobe 3 unidades

        while (elapsedTime < 0.5f)
        {
            spike.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime * attackSpeed;
            yield return null;
        }
        */
        // Espera um tempo antes de destruir o espinho
        yield return new WaitForSeconds(destroyTime);
        spikeAttack.enabled = true;
        /*
        elapsedTime = 0f;

        while (elapsedTime < 0.5f)
        {
            spike.transform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime * attackSpeed;
            yield return null;
        }
        */
        yield return new WaitForSeconds(0.5f);
        spikeAttack.enabled = false;
        animArraia.SetBool("ArraiaAtaca", false);
        Destroy(spike);
        attackIndex = 0;
    }

    // Função para embaralhar uma lista de índices
    private void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private void ChangeState()
    {
        if (!transitioning)
        {
            attackIndex = Random.Range(1, 5);
        }
    }
}