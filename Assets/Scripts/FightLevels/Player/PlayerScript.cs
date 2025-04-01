using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public Transform spr;
    private InputSystem_Actions inputActions;
    public Rigidbody2D rb;
    public Animator anim;
    private bool Attacking = false;
    public GameObject AttackArea;
    public int damage = 0;
    private int life = 100;
    private bool CanMove = true;
    private bool Defending = false;
    [SerializeField] TMP_Text Lifetext;
    [SerializeField] private GameObject popUpPrefab;
    void Start()
    {
        Lifetext.text = life.ToString();
    }
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
        inputActions.Player.Defend.Enable();
        inputActions.Player.Defend.performed += Ondefend;

    }

    private void OnDisable()
    {
        //idem 
        inputActions.Player.Disable();
        inputActions.Player.Move.Disable();
        inputActions.Player.Attack.Disable();
        inputActions.Player.Attack.performed -= Onattack;
        inputActions.Player.Defend.Disable();
        inputActions.Player.Defend.performed -= Ondefend;
    }
    public float speed = 5f;
    public float jump_force = 7f;

    // Update is called once per frame
    void Update()
    {
        Vector2 move_input = inputActions.Player.Move.ReadValue<Vector2>();
        // lê input do gamepad/teclas
        if (Attacking)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y);
        }

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
        if (!Attacking && CanMove && !Defending)
        {
            //dá set do ataque de terra
            damage = 20;
            AttackArea.SetActive(true);
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y);
            anim.SetBool("Punching", true);
            Attacking = true;
            StartCoroutine(AttackTiming());
        }
    }

    private void Ondefend(InputAction.CallbackContext context)
    {
        // verifica se pode defender e se o botão está largado
        if (!Attacking)
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


    IEnumerator AttackTiming()
    {
        float attackTime = anim.GetCurrentAnimatorStateInfo(0).length; // pega no tempo que a anim demora
        yield return new WaitForSeconds(attackTime);
        anim.SetBool("Punching", false);
        Attacking = false;
        AttackArea.SetActive(false);
    }

    public void Attacked(int value)
    {
        if (!Defending)
        {
            life -= value;
            GameObject popUp = Instantiate(popUpPrefab, rb.transform.position, Quaternion.identity);
                    popUp.GetComponentInChildren<TMP_Text>().text = value.ToString();
            Lifetext.text = life.ToString();
            if (life <= 0)
            {
                life = 0;
                Destroy(gameObject);
            }
            else
            {
                // anim.SetBool("Hurt", true);
                StartCoroutine(HurtTimer());
            }
        }
    }
    IEnumerator HurtTimer()
    {
        CanMove = false;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - .2f);
        CanMove = true;
        // anim.SetBool("Hurt", false);
    }
}
