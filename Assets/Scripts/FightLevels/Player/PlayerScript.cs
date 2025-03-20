using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;

public class PlayerScript : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    public float speed = 15f;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private Animator anim;
    private bool CanMove;
    private bool Punching;
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
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move_input = inputActions.Player.Move.ReadValue<Vector2>();
        if (CanMove == true)
        {
            rb.linearVelocity = new Vector2(move_input.x * speed, rb.linearVelocity.y);
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
        Punching = true;
        anim.SetBool("Punching", true);
        CanMove = false;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        CanMove = true;
        anim.SetBool("Punching", false);
        Punching = false;
    }
}
