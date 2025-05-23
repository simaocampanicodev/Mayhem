using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.Rendering.Universal;

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
    private const int MAXLIFE = 100;
    private bool CanMove = true;
    private bool Defending = false;
    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private Image lifebar;
    [SerializeField] private GameObject particles;
    public bool Uppercut = false;
    [SerializeField] private AudioSource radioSource;
    [SerializeField] private GruntScript hurtSound;
    [SerializeField] private GameObject cadaver;
    public int multiplier = 1;
    private bool right;

    public int BeatenEnemies { get; set; }
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
        inputActions.Player.Defend.started += Ondefend;
        inputActions.Player.Defend.canceled += Ondefend;

    }

    private void OnDisable()
    {
        //idem 
        inputActions.Player.Disable();
        inputActions.Player.Move.Disable();
        inputActions.Player.Attack.Disable();
        inputActions.Player.Attack.performed -= Onattack;
        inputActions.Player.Defend.Disable();
        inputActions.Player.Defend.started -= Ondefend;
        inputActions.Player.Defend.canceled -= Ondefend;
    }
    public float speed = 10f;
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

        if (CanMove == true && !Defending)
        {
            rb.linearVelocity = new Vector2(move_input.x * speed * multiplier, rb.linearVelocity.y);
            if (move_input.x > 0)
            {
                transform.rotation = Quaternion.identity;
                anim.SetBool("Walk", true);
            }
            else if (move_input.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }
        }
    }

    private void Onattack(InputAction.CallbackContext context)
    {
        if (!Attacking && !Defending)
        {
            anim.SetBool("Walk", false);
            //dá set do ataque de terra
            Vector2 move_input = inputActions.Player.Move.ReadValue<Vector2>();
            CanMove = false;
            ToggleHand();
            if (right)
            {
                anim.SetBool("Right", true);
            }
            else
            {
                anim.SetBool("Right", false);
            }
            if (move_input.y > 0)
            {
                damage = 30 * multiplier;
                anim.SetBool("Uppercut", true);
            }
            else if (move_input.y < 0)
            {
                damage = 30 * multiplier;
                anim.SetBool("Downwards", true);
            }
            else
            {
                damage = 20 * multiplier;
                anim.SetBool("Punching", true);
            }
            AttackArea.SetActive(true);
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y);
            Attacking = true;
            if (move_input.y > 0)
            {
                StartCoroutine(UppercutTiming());
            }
            else
            {
                StartCoroutine(AttackTiming());
            }
        }
    }

    private void Ondefend(InputAction.CallbackContext context)
    {
        // verifica se pode defender e se o botão está largado
        if (!Attacking)
        {
            if (context.started)
            {
                anim.SetBool("Block", true);
                Defending = true;
                CanMove = false;
            }
            if (context.canceled)
            {
                anim.SetBool("Block", false);
                Defending = false;
                CanMove = true;
            }
        }
    }


    IEnumerator AttackTiming()
    {
        float attackTime = anim.GetCurrentAnimatorStateInfo(0).length; // pega no tempo que a anim demora
        yield return new WaitForSeconds(attackTime);
        anim.SetBool("Downwards", false);
        anim.SetBool("Punching", false);
        Attacking = false;
        CanMove = true;
        AttackArea.SetActive(false);
    }

    IEnumerator UppercutTiming()
    {
        Uppercut = true;
        float attackTime = anim.GetCurrentAnimatorStateInfo(0).length; // pega no tempo que a anim demora
        yield return new WaitForSeconds(attackTime);
        anim.SetBool("Uppercut", false);
        Uppercut = false;
        Attacking = false;
        CanMove = true;
        AttackArea.SetActive(false);
    }

    public void Attacked(int value)
    {
        GameObject blood = Instantiate(particles, transform.position, transform.rotation);
        Debug.Log("Defending: " + Defending);
        GameObject popUp = Instantiate(popUpPrefab, rb.transform.position, Quaternion.identity);
        if (!Defending)
        {
            life -= value;
            popUp.GetComponentInChildren<TMP_Text>().text = value.ToString();
        }
        else if (Defending)
        {
            life -= value / 3;
            popUp.GetComponentInChildren<TMP_Text>().text = (value / 3).ToString();
        }
        float lifePercent = Mathf.Clamp01((float)life / MAXLIFE);
        lifebar.fillAmount = lifePercent;

        if (life <= 0)
        {
            AudioClip grunt = hurtSound.DeathSound; // gera um número entre 0 e o final da lista
            radioSource.PlayOneShot(grunt); // escolhe um audio da lista de acordo com esse número
            life = 0;
            Instantiate(cadaver, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            AudioClip grunt = hurtSound.GruntSound; // gera um número entre 0 e o final da lista
            radioSource.PlayOneShot(grunt); // escolhe um audio da lista de acordo com esse número
            anim.SetBool("Hurt", true);
            StartCoroutine(HurtTimer(blood));

        }
    }
    IEnumerator HurtTimer(GameObject blood)
    {
        CanMove = false;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - .2f);
        anim.SetBool("Hurt", false);
        CanMove = true;
        Destroy(blood);
    }

    void ToggleHand()
    {
        if (right == false)
        {
            right = true;
        }
        else if (right == true)
        {
            right = false;
        }
    }
}
