using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    [SerializeField] public float speed = 15f;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private Animator anim;
    private bool CanMove;
    private bool Punching;
    private bool Defending;
    [SerializeField] private GameObject punch;
    public int punchDamage = 20;
    [SerializeField] private GameObject colpunch;
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
        inputActions.Player.Defend.Enable();
        inputActions.Player.Attack.performed += Onattack;
        inputActions.Player.Defend.started += Ondefend;
        inputActions.Player.Defend.canceled += Ondefend;
    }
    private void OnDisable()
    {
        //idem 
        inputActions.Player.Disable();
        inputActions.Player.Move.Disable();
        inputActions.Player.Attack.Disable();
        inputActions.Player.Defend.Disable();
        inputActions.Player.Attack.performed -= Onattack;
        inputActions.Player.Defend.started -= Ondefend;
        inputActions.Player.Defend.canceled -= Ondefend;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // lê o x axis e inverte o sprite
        Vector2 move_input = inputActions.Player.Move.ReadValue<Vector2>();
        if (CanMove == true)
        {
            rb.linearVelocity = new Vector2(move_input.x * speed, rb.linearVelocity.y);
            if (move_input.x > 0)
            {
                transform.rotation = Quaternion.identity;
            }
            else if (move_input.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    private void Onattack(InputAction.CallbackContext context)
    {
        // verifica se pode esmurrar
        if (!Punching && !Defending)
        {
            // inicia corotina
            StartCoroutine(PunchTimer());
        }
    }

    private void Ondefend(InputAction.CallbackContext context)
    {
        // verifica se pode defender e se o botão está largado
        if (!Punching)
        {
            if (context.started)
            {
                Defending = true;
                CanMove = false;
            }
            if (context.canceled)
            {
                Defending = false;
                CanMove = true;
            }
        }
    }

    IEnumerator PunchTimer()
    {
        punch.SetActive(true);
        Punching = true;
        anim.SetBool("Punching", true);
        CanMove = false;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        CanMove = true;
        anim.SetBool("Punching", false);
        Punching = false;
        punch.SetActive(false);
    }
}