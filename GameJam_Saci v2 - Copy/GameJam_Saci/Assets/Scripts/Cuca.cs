using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuca : MonoBehaviour
{

    [Header("Cuca Settings")]
    public Transform positionOrigem;
    public int attackIndex;
    public float timeBetweenAttacks = 3f;   // Tempo entre os estados/ataques
    public Animator anim;
    public SpriteRenderer cuca;

    private bool transitioning = false;

    [Header("BubbleAttack Settings")]
    public GameObject bubblePrefab;
    public float attackCooldown = 1f;
    private float attackTimer;

    [Header("PursuitAttack Settings")]
    public float acceleration = 5f; // Aceleração do boss
    public float maxSpeed = 10f; // Velocidade máxima do boss
    public float deceleration = 5f; // Derrapagem ao errar o jogador
    public float pursuitDuration = 20f; // Tempo de perseguição
    public float exitSpeed = 15f; // Velocidade para sair da tela
    public Transform PursuitPoint;

    private bool isChasing = false;
    private float lastPlayerX; // Guarda a última posição do jogador

    [Header("JumpScareAttack Settings")]
    public GameObject spikePrefab;      // Prefab do espinho
    public float spikeDelay = 0.1f;     // Tempo de delay antes do espinho atacar
    public float attackInterval = 1.75f;   // Intervalo entre ataques
    public int maxAttacks = 5;          // Número máximo de ataques
    public Transform ground;

    [Header("BubblePlatformAttack Settings")]
    public GameObject platformPrefab; // Prefab da plataforma
    public Transform[] spawnPoints; // Locais onde as plataformas surgem
    public float spawnInterval = 2f; // Tempo entre os spawns
    public float fallSpeed = 3f; // Velocidade de queda das plataformas
    public float destroyY = -6f; // Posição onde as plataformas são destruídas
    public GameObject groundOk; // Chão seguro
    public GameObject groundDangered; // Chão perigoso

    public Rigidbody2D rb;
    private float speed = 0f;
    private int currentAttackCount = 0; // Contador de ataques realizados
    private bool isAttacking = false;   // Indica se o ataque está em progresso
    public Transform player;           // Referência ao jogador
    private float currentSpeed = 0f;   // Velocidade atual do boss


    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine(StateMachine());
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cuca.tag == "AtkCuca2") 
        {
            if (gameObject.transform.position.x > player.transform.position.x)
            {
                cuca.flipX = true;
            }
            else
            {
                cuca.flipX = false;
            }
        }
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
                    yield return StartCoroutine(BubbleAttack(12));
                    break;

                case 2:
                    yield return StartCoroutine(PursuingAttack());
                    break;

                case 3:
                    yield return StartCoroutine(JumpScareAttack());
                    break;

                case 4:
                    yield return StartCoroutine(SpawnPlatforms());
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

    private IEnumerator BubbleAttack(int bubblesCount)
    {
        int attackController = 0;

        while (attackController < bubblesCount)
        {
            float yVariation = Random.Range(-2f, 1f);
            Transform position = positionOrigem;
            position.position = new Vector2(position.position.x, (position.position.y + yVariation));

            Instantiate(bubblePrefab, position.position, Quaternion.identity);

            yield return new WaitForSeconds(1f);

            position.position = gameObject.transform.position;

            attackController++;
        }
        if (attackIndex == 1)
        {
            attackIndex = 0;
        }


    }

    private IEnumerator PursuingAttack()
    {
        anim.SetBool("Drinking", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Drinking", false);
        anim.SetBool("Pursuit", true);

        cuca.tag = "AtkCuca2";


        gameObject.transform.position = PursuitPoint.position;
        isChasing = true;
        float timer = 0f;
        lastPlayerX = player.position.x;

        while (timer < pursuitDuration)
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            float targetVelocity = direction * maxSpeed;

            // Se o jogador mudar de direção bruscamente, o Boss derrapa
            if (Mathf.Abs(player.position.x - lastPlayerX) > 2f)
            {
                //cuca.flipX = true;

                

                yield return StartCoroutine(Skid());
            }

            // Move o Boss na direção do jogador
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, targetVelocity, acceleration * Time.deltaTime), rb.velocity.y);

            lastPlayerX = player.position.x; // Atualiza a última posição do jogador
            timer += Time.deltaTime;
            yield return null;
        }

        isChasing = false;
        yield return StartCoroutine(ExitScreen());

        cuca.tag = "Untagged";

        anim.SetBool("Pursuit", false);
    }

    private IEnumerator Skid()
    {

        float skidTime = 0.5f; // Tempo de derrapagem
        float timer = 0f;

        while (timer < skidTime)
        {
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, deceleration * Time.deltaTime), rb.velocity.y);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ExitScreen()
    {
        float direction = transform.position.x < 0 ? -1 : 1; // Define para onde o boss sai
        rb.velocity = new Vector2(direction * exitSpeed, rb.velocity.y);

        yield return new WaitForSeconds(1f); // Tempo para sair da tela
        gameObject.transform.position = positionOrigem.position;
        rb.velocity = new Vector2(0, rb.velocity.y);
        attackIndex = 0;
    }

    private IEnumerator JumpScareAttack()
    {
        while (currentAttackCount < maxAttacks)
        {
            cuca.enabled = false;
            if (!isAttacking)
            {
                isAttacking = true;

                float elapsedTime = 0f;

                // Posição do espinho no mesmo eixo X do jogador
                Vector3 spikePosition = new Vector3(player.position.x, (ground.position.y), 0f); // Ajuste o eixo Y para a posição inicial do espinho

                // Instanciar o espinho
                GameObject spike = Instantiate(spikePrefab, spikePosition, Quaternion.identity);

                // Delay antes do ataque para dar tempo de esquiva
                yield return new WaitForSeconds(spikeDelay);

                Vector3 startPosition = spike.transform.position;
                Vector3 targetPosition = startPosition + Vector3.up * 4f;

                // Subir o espinho
                while (elapsedTime < 0.5f)
                {
                    spike.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.5f);
                    elapsedTime += Time.deltaTime * 10f;
                    yield return null;
                }

                elapsedTime = 0f;

                // Aguarda o tempo do ataque
                yield return new WaitForSeconds(0.75f);

                while (elapsedTime < 0.5f)
                {
                    spike.transform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime / 0.5f);
                    elapsedTime += Time.deltaTime * 10f;
                    yield return null;
                }

                // Destruir o espinho ou desativá-lo
                Destroy(spike);

                // Incrementa o contador de ataques
                currentAttackCount++;

                isAttacking = false;
            }

            yield return null;
        }
        cuca.enabled = true;
        attackIndex = 0;
    }

    private IEnumerator SpawnPlatforms()
    {
        StartCoroutine(PlatformAttackStart());
        int platformCount = 0;
        while (platformCount < 8) // Loop infinito para spawn contínuo
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Transform spawnPoint2 = spawnPoint;
            while (spawnPoint2 == spawnPoint)
            {
                spawnPoint2 = spawnPoints[Random.Range(0, spawnPoints.Length)];
            }
            GameObject platform = Instantiate(platformPrefab, spawnPoint.position, Quaternion.identity);
            StartCoroutine(MovePlatform(platform));
            platform = Instantiate(platformPrefab, spawnPoint2.position, Quaternion.identity);
            StartCoroutine(MovePlatform(platform));

            StartCoroutine(BubbleAttack(2));

            yield return new WaitForSeconds(spawnInterval); // Aguarda 2 segundos antes do próximo spawn]
            platformCount++;
        }
        groundOk.SetActive(true);
        attackIndex = 0;
    }

    private IEnumerator MovePlatform(GameObject platform)
    {
        while (platform != null)
        {
            platform.transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // Destroi a plataforma quando sair da tela
            if (platform.transform.position.y < destroyY)
            {
                Destroy(platform);
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator PlatformAttackStart()
    {
        yield return new WaitForSeconds(1.5f);
        groundOk.SetActive(false);

    }

    private void ChangeState()
    {
        if (!transitioning)
        {
            attackIndex = Random.Range(1, 4);
        }
    }

}