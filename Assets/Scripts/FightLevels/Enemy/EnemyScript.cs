using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            Debug.Log("Rigidbody2D adicionado ao inimigo");
        }

        // Configurar propriedades do Rigidbody2D - manter gravidade normal
        rb.freezeRotation = true;

        // Garantir que tenha um collider
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
            Debug.Log("Collider adicionado ao inimigo");
        }

        currentHealth = maxHealth;

        // Tag para identificação
        gameObject.tag = "Enemy";
        Debug.Log("Inimigo inicializado com tag: " + gameObject.tag);
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
            // Mostra o texto de dano somente se a classe DamageDisplay existir
            if (System.Type.GetType("DamageDisplay") != null)
            {
                DamageDisplay.Create(transform.position + Vector3.up, damage);
            }
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

        // Verificar se o outro objeto é válido
        if (other == null || !other.gameObject.activeSelf)
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
}
