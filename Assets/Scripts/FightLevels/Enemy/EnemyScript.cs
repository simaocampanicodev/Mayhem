using UnityEngine;
using System.Collections;
using TMPro;
using UnityEditor.Animations;

public class EnemyScript : MonoBehaviour
{
    private int life = 80;
    private bool IsAttacked = false;
    public Animator anim;
    private AnimatorController animC;
    public bool ChaseMode = false;
    public bool Attacking = false;
    private Transform target;
    public Rigidbody2D rb;
    public float speed = 5f;
    public Transform spr;
    private PlayerScript player;
    public int damage = 20;
    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private AudioSource punchsound;
    [SerializeField] private GameObject particles;
    [SerializeField] private AudioSource radioSource;
    [SerializeField] private GruntScript hurtSound;
    private bool canJuggle = false;
    private TypeEnemy typeenemy;
    public AnimatorController whiteAnimator;
    public AnimatorController blackAnimator;
    public AnimatorController asianAnimator;
    private bool Blocking = false;

    void Start()
    {
        // pega nos componentes do jogador
        typeenemy = (TypeEnemy)Random.Range(0, 3);
        switch (typeenemy)
        {
            case TypeEnemy.JaneDoe:
                animC = whiteAnimator;
                break;
            case TypeEnemy.Tyrone:
                animC = blackAnimator;
                break;
            case TypeEnemy.Wang:
                animC = asianAnimator;
                break;
        }

        anim.runtimeAnimatorController = animC;
        target = GameObject.FindWithTag("Player").transform;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (life <= 0)
        {
            AudioClip grunt = hurtSound.DeathSound;
            radioSource.PlayOneShot(grunt);
            Destroy(gameObject);
        }
        if (ChaseMode && !Attacking && !IsAttacked && !Blocking)
        {
            if (transform.position.x < target.position.x)
            {
                anim.SetBool("Move", true);
                rb.linearVelocity = new Vector3(speed, rb.linearVelocity.y);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            if (transform.position.x > target.position.x)
            {
                anim.SetBool("Move", true);
                rb.linearVelocity = new Vector3(-speed, rb.linearVelocity.y);
                transform.rotation = Quaternion.identity;
            }
        }
        if (!ChaseMode || Attacking || IsAttacked || Blocking)
        {
            anim.SetBool("Move", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //atacar o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!IsAttacked && !Attacking && !Blocking) //verificar se já começou a atacar
            {
                ChooseMove();
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Punch"))
        { //é atacado normalmente
            if (!IsAttacked)
            {
                Attacking = false;
                if (!Blocking) { life -= player.damage; }
                if (Blocking) { life -= player.damage / 3; }
                GameObject popUp = Instantiate(popUpPrefab, rb.transform.position, Quaternion.identity);
                popUp.GetComponentInChildren<TMP_Text>().text = player.damage.ToString();
                IsAttacked = true;
                Debug.Log(life);
                StartCoroutine(AttackedPool());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJuggle = false;
        }
    }

    IEnumerator AttackedPool()
    {
        StopCoroutine(AttackEnemy());
        StopCoroutine(BlockEnemy());
        anim.SetBool("Punching", false);
        anim.SetBool("Blocking", false);

        if (life != 0)
        {
            AudioClip grunt = hurtSound.GruntSound;
            radioSource.PlayOneShot(grunt);
        }
        PlayerScript plr = FindAnyObjectByType<PlayerScript>();
        anim.SetBool("Move", false);
        GameObject blood = Instantiate(particles, transform.position, transform.rotation);
        // anim.SetBool("Move", false);
        // anim.SetBool("Hurt", true);
        if (plr.Uppercut == true && !Blocking)
        {
            canJuggle = true;
            rb.AddForce(transform.up * 20, ForceMode2D.Impulse);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        punchsound.Play();
        // anim.SetBool("Hurt", false);
        yield return new WaitForSeconds(.5f);
        IsAttacked = false;
        Destroy(blood);
    }

    private void ChooseMove()
    {
        int choice = Random.Range(0, 2);
        switch (choice)
        {
            case 0:
                StartCoroutine(AttackEnemy());
                break;
            case 1:
                StartCoroutine(BlockEnemy());
                break;
        }
    }

    IEnumerator AttackEnemy()
    {
        Attacking = true;
        anim.SetBool("Punching", true);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        player.Attacked(damage);
        punchsound.Play();
        anim.SetBool("Punching", false);
        Attacking = false;
        if (!ChaseMode)
        {
            ChooseMove();
        }
    }

    IEnumerator BlockEnemy()
    {
        Blocking = true;
        anim.SetBool("Blocking", true);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Blocking = false;
        anim.SetBool("Blocking", false);
        if (!ChaseMode)
        {
            ChooseMove();
        }
    }
}