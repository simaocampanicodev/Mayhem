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
    private bool CanMove = true;
    private bool Defending = false;
    [SerializeField] private Image LifeUI;
    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private Sprite[] lifebarList;
    [SerializeField] private GameObject particles;
    public bool Uppercut = false;
    [SerializeField] private AudioSource radioSource;
    [SerializeField] private GruntScript hurtSound;
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

        if (CanMove == true)
        {
            rb.linearVelocity = new Vector2(move_input.x * speed, rb.linearVelocity.y);
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
            else {
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
            if (move_input.y > 0) {
                damage = 30;
                anim.SetBool("Uppercut", true);
            }
            else {
                damage = 20;
                anim.SetBool("Punching", true);
            }
            AttackArea.SetActive(true);
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y);
            Attacking = true;
            if (move_input.y > 0) {
                StartCoroutine(UppercutTiming());
            }
            else {
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
            popUp.GetComponentInChildren<TMP_Text>().text = (value/3).ToString();
        }
        int maxLife = 100; // max de vida
        float lifePercent = (float)life / maxLife; // percentagem de vida dividido pelo maximo
        int spriteNum = Mathf.Clamp((int)(lifePercent * lifebarList.Length), 0, lifebarList.Length - 1); // escolhe da lista a imagem consoante a percentagem de vida, e apróxima-la para um int        
        LifeUI.sprite = lifebarList[spriteNum];
        if (life <= 0)
        {
            AudioClip grunt = hurtSound.DeathSound; // gera um número entre 0 e o final da lista
            radioSource.PlayOneShot(grunt); // escolhe um audio da lista de acordo com esse número
            life = 0;
            Destroy(gameObject);
        }
        else
        {
            AudioClip grunt = hurtSound.GruntSound; // gera um número entre 0 e o final da lista
            radioSource.PlayOneShot(grunt); // escolhe um audio da lista de acordo com esse número
            // anim.SetBool("Hurt", true);
            StartCoroutine(HurtTimer(blood));

        }
    }
    IEnumerator HurtTimer(GameObject blood)
    {
        CanMove = false;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - .2f);
        CanMove = true;
        Destroy(blood);
        // anim.SetBool("Hurt", false);
    }
}
