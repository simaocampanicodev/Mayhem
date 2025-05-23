using UnityEngine;
using System.Collections;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    private int life = 80;
    private bool IsAttacked = false;
    private Animator anim;
    private RuntimeAnimatorController animC;
    public bool ChaseMode = false;
    public bool Attacking = false;
    private Transform target;
    public Rigidbody2D rb;
    public float speed = 5f;
    public Transform spr;
    private PlayerScript player;
    public int damage = 25;
    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private AudioSource punchsound;
    [SerializeField] private GameObject particles;
    [SerializeField] private AudioSource radioSource;
    [SerializeField] private GruntScript hurtSound;
    private bool canJuggle = false;
    private TypeEnemy typeenemy;
    public RuntimeAnimatorController whiteAnimator;
    public RuntimeAnimatorController blackAnimator;
    public RuntimeAnimatorController asianAnimator;
    [SerializeField] private GameObject JaneBody;
    [SerializeField] private GameObject TyroneBody;
    [SerializeField] private GameObject WangBody;
    private GameObject Body;
    private bool Blocking = false;
    private PlayerScript plr;
    private bool WithinPlayer;

    void Start()
    {
        anim = GetComponent<Animator>();
        plr = FindAnyObjectByType<PlayerScript>();
        // pega nos componentes do jogador
        typeenemy = (TypeEnemy)Random.Range(0, 3);
        switch (typeenemy)
        {
            case TypeEnemy.JaneDoe:
                animC = whiteAnimator;
                Body = JaneBody;
                break;
            case TypeEnemy.Tyrone:
                animC = blackAnimator;
                Body = TyroneBody;
                break;
            case TypeEnemy.Wang:
                animC = asianAnimator;
                Body = WangBody;
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
            Instantiate(Body, transform.position, transform.rotation);
            AudioClip grunt = hurtSound.DeathSound;
            radioSource.PlayOneShot(grunt);
            plr.BeatenEnemies += 1;
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
                WithinPlayer = true;
                ChooseMove();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //atacar o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            WithinPlayer = false;
            if (!Attacking) { CancelAll(); }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Punch"))
        { //é atacado normalmente
            if (!IsAttacked)
            {
                IsAttacked = true;
                if (!Blocking) { life -= player.damage; }
                if (Blocking) { life -= player.damage / 3; }
                GameObject popUp = Instantiate(popUpPrefab, rb.transform.position, Quaternion.identity);
                popUp.GetComponentInChildren<TMP_Text>().text = player.damage.ToString();
                Debug.Log(life);
                StartCoroutine(AttackedPool());
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsAttacked && !Attacking) //verificar se já começou a atacar
            {
                CancelAll();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJuggle = false;
            anim.SetBool("Air", false);
        }
    }

    IEnumerator AttackedPool()
    {
        if (life != 0)
        {
            AudioClip grunt = hurtSound.GruntSound;
            radioSource.PlayOneShot(grunt);
        }
        anim.SetBool("Move", false);
        GameObject blood = Instantiate(particles, transform.position, transform.rotation);
        if (canJuggle && plr.Uppercut == true)
        {
            anim.SetBool("Air", false);
            anim.SetBool("Air", true);
            rb.AddForce(transform.up * 25, ForceMode2D.Impulse);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        if (plr.Uppercut == true && !Blocking && !canJuggle)
        {
            anim.SetBool("Air", false);
            anim.SetBool("Air", true);
            canJuggle = true;
            rb.AddForce(transform.up * 20, ForceMode2D.Impulse);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            anim.SetBool("Hit", true);
        }
        punchsound.Play();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("Hit", false);
        IsAttacked = false;
        Destroy(blood);
    }

    private void ChooseMove()
    {
        if (!IsAttacked)
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
    }

    IEnumerator AttackEnemy()
    {
        Attacking = true;
        anim.SetBool("Punching", true);
        yield return new WaitForSeconds(.5f);
        if (!IsAttacked && WithinPlayer)
        {
            player.Attacked(damage);
            punchsound.Play();
        }
        anim.SetBool("Punching", false);
        Attacking = false;
        if (WithinPlayer)
        {
            ChooseMove();
        }
    }

    IEnumerator BlockEnemy()
    {
        Blocking = true;
        anim.SetBool("Blocking", true);
        yield return new WaitForSeconds(.5f);
        Blocking = false;
        anim.SetBool("Blocking", false);
        if (WithinPlayer)
        {
            ChooseMove();
        }
    }

    private void CancelAll()
    {
        Attacking = false;
        StopCoroutine(AttackEnemy());
        StopCoroutine(BlockEnemy());
        anim.SetBool("Blocking", false);
        anim.SetBool("Punching", false);
    }
}