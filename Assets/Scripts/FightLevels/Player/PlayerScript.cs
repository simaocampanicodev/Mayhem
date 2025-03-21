using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    [Header("Movimento")]
    public float speed = 15f;

    [Header("Combate")]
    public int punchDamage = 20;

    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private Animator anim;
    private bool CanMove = true;
    private bool Punching;

    public GameObject punch;

    void Awake()
    {
        //lê os inputs
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        //verifica se o jogador clicou nas setas/joystick ou espaço/cruz
        inputActions.Player.Enable();
        inputActions.Player.Move.Enable();
        inputActions.Player.Attack.Enable();
        inputActions.Player.Attack.performed += Onattack;
    }

    private void OnDisable()
    {
        //idem 
        inputActions.Player.Disable();
        inputActions.Player.Move.Disable();
        inputActions.Player.Attack.Disable();
        inputActions.Player.Attack.performed -= Onattack;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Verificar e obter componentes necessários
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            Debug.Log("Rigidbody2D adicionado ao player");
        }

        // Configurações do Rigidbody - com gravidade normal para cair
        rb.freezeRotation = true;

        // Verificar se tem collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
            Debug.Log("BoxCollider2D adicionado ao player");
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("Animator não encontrado no player");
        }

        spr = GetComponent<SpriteRenderer>();
        if (spr == null)
        {
            Debug.LogWarning("SpriteRenderer não encontrado no player");
        }

        // Configura a tag para permitir que o inimigo encontre o player
        gameObject.tag = "Player";

        // Verificar punch
        if (punch == null)
        {
            Debug.LogWarning("Objeto de punch não está configurado no inspector!");
        }
        else
        {
            // Garantir que o punch começa desativado
            punch.SetActive(false);

            // Configurar o trigger do punch
            Collider2D punchCollider = punch.GetComponent<Collider2D>();
            if (punchCollider == null)
            {
                BoxCollider2D boxCollider = punch.AddComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(1.5f, 1.5f); // Aumentar um pouco para melhorar a detecção
                boxCollider.isTrigger = true;
                Debug.Log("Collider adicionado ao punch com tamanho: " + boxCollider.size);
            }
            else
            {
                punchCollider.isTrigger = true;
                Debug.Log("Configurado punch collider como trigger");
            }

            // Tag para detecção de golpe
            punch.tag = "Punch";
            Debug.Log("Punch configurado com tag: " + punch.tag);
        }
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        if (CanMove)
        {
            Vector2 move_input = inputActions.Player.Move.ReadValue<Vector2>();
            rb.linearVelocity = new Vector2(move_input.x * speed, rb.linearVelocity.y);

            // Virar sprite baseado na direção
            if (move_input.x > 0)
            {
                spr.flipX = false;
            }
            else if (move_input.x < 0)
            {
                spr.flipX = true;
            }
        }
    }

    private void Onattack(InputAction.CallbackContext context)
    {
        if (!Punching)
        {
            StartCoroutine(PunchTimer());
        }
    }

    IEnumerator PunchTimer()
    {
        if (punch != null)
        {
            Debug.Log("Ativando punch");
            punch.SetActive(true);
            Punching = true;
            CanMove = false;

            if (anim != null)
            {
                anim.SetBool("Punching", true);
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                // Se não tiver animator, espera um tempo fixo
                yield return new WaitForSeconds(0.5f);
            }

            CanMove = true;

            if (anim != null)
            {
                anim.SetBool("Punching", false);
            }

            Punching = false;
            punch.SetActive(false);
            Debug.Log("Desativando punch");
        }
        else
        {
            yield return null;
        }
    }
}
