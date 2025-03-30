using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using TMPro;
using System;

public class EnemyScript : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    private Rigidbody2D rb;
    private bool isDead = false;
    public GameObject popUpPrefab;
    public bool ChaseMode = false;
    private Transform target;
    public float speed = 10;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        rb.freezeRotation = true;

        currentHealth = maxHealth;

        // Tag para identificação
        gameObject.tag = "Enemy";
        Debug.Log("Inimigo inicializado com tag: " + gameObject.tag);
    }

    void Update()
    {
        if (ChaseMode)
        {
            if (transform.position.x < target.position.x)
            {
                rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            }

            if (transform.position.x > target.position.x)
            {
                rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        Debug.Log("Inimigo tomou " + damage + " de dano! Vida: " + currentHealth);

        // Tente mostrar o texto de dano, mas com proteção contra erro
        try
        {
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erro ao criar texto de dano: " + e.Message);
        }

        // Verifica se morreu
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // Evita chamar Die() múltiplas vezes

        isDead = true;
        Debug.Log("Inimigo morreu!");

        // ALTERNATIVA SIMPLIFICADA:
        // Destruir o objeto imediatamente
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar se o objeto está ativo e não está destruído
        if (!this.isActiveAndEnabled || isDead)
            return;

        if (other.gameObject.CompareTag("Punch"))
        {
            Debug.Log("Punch detectado!");
            // Encontra o script do player para pegar o valor de dano
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerScript playerScript = player.GetComponent<PlayerScript>();
                if (playerScript != null)
                {
                    GameObject popUp = Instantiate(popUpPrefab, rb.transform.position, Quaternion.identity);
                    popUp.GetComponentInChildren<TMP_Text>().text = playerScript.punchDamage.ToString();
                    TakeDamage(playerScript.punchDamage);
                }
                else
                {
                    // Valor padrão se não conseguir acessar o script do player
                    TakeDamage(20);
                }
            }
            else
            {
                // Se não encontrar o player, usa o valor padrão
                TakeDamage(20);
            }
        }
    }

    private Vector3 Vector3(int v1, int v2, int v3)
    {
        throw new NotImplementedException();
    }

    private Vector3 Vector2(int v1, int v2, int v3)
    {
        throw new NotImplementedException();
    }
}
