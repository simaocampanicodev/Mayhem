using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;

public class EnemyScript : MonoBehaviour
{
    private int life = 80;
    private bool IsAttacked = false;
    public Animator anim;
    private AnimatorController animC;
    public float pool = 1f;
    public bool ChaseMode = false;
    public bool Attacking = false;
    private Transform target;
    public Rigidbody2D rb;
    public float speed = 5f;
    public Transform spr;
    private PlayerScript player;
    private bool ATKrunning = false;
    public int damage = 20;
    public Collider2D owntrigger;
    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private AudioSource punchsound;
    [SerializeField] private GameObject particles;
    [SerializeField] private AudioSource radioSource;
    [SerializeField] private GruntScript hurtSound;
    public float timeFrame = .1f;
    private bool canJuggle = false;
    private TypeEnemy typeenemy;
    public AnimatorController whiteAnimator;
    public AnimatorController blackAnimator;
    public AnimatorController asianAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // pega nos componentes do jogador
        typeenemy = (TypeEnemy)Random.Range(0, 3);
        switch (typeenemy)
        {
            case TypeEnemy.White:
                animC = whiteAnimator;
                break;
            case TypeEnemy.Black:
                animC = blackAnimator;
                break;
            case TypeEnemy.Asian:
                animC = asianAnimator;
                break;
        }

        anim.runtimeAnimatorController = animC;
        target = GameObject.FindWithTag("Player").transform;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            AudioClip grunt = hurtSound.DeathSound;
            radioSource.PlayOneShot(grunt);
            Destroy(gameObject);
        }
        //código para o inimigo perseguir o jogador
        if (ChaseMode && !Attacking && !IsAttacked)
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
        if (!ChaseMode && !Attacking && !IsAttacked)
        {
            anim.SetBool("Move", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //atacar o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!ATKrunning && !IsAttacked) //verificar se já começou a atacar
            {
                anim.SetBool("Punching", true);
                ATKrunning = true;
                StartCoroutine(AttackEnemy());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Punch"))
        { //é atacado normalmente
            if (!IsAttacked)
            {
                anim.SetBool("Punching", false);
                Attacking = false;
                StopCoroutine(AttackEnemy());
                ATKrunning = false;
                life -= player.damage;
                GameObject popUp = Instantiate(popUpPrefab, rb.transform.position, Quaternion.identity);
                popUp.GetComponentInChildren<TMP_Text>().text = player.damage.ToString();
                IsAttacked = true;
                Debug.Log(life);
            }
            StartCoroutine(AttackedPool());
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
        if (life != 0)
        {
            AudioClip grunt = hurtSound.GruntSound;
            radioSource.PlayOneShot(grunt);
        }
        PlayerScript plr = FindAnyObjectByType<PlayerScript>();
        anim.SetBool("Move", false);
        GameObject blood = Instantiate(particles, transform.position, transform.rotation);
        owntrigger.enabled = false;
        // anim.SetBool("Move", false);
        // anim.SetBool("Hurt", true);
        if (plr.Uppercut == true && canJuggle == false)
        {
            canJuggle = true;
            rb.AddForce(transform.up * 15, ForceMode2D.Impulse);
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y);
        }
        if (canJuggle == true)
        {
            rb.AddForce(transform.up * 10, ForceMode2D.Impulse);
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y);
        }
        yield return new WaitForSeconds(pool - .1f);
        punchsound.Play();
        // anim.SetBool("Hurt", false);
        IsAttacked = false;
        owntrigger.enabled = true;
        Destroy(blood);
    }

    IEnumerator AttackEnemy()
    {
        //ataca
        ATKrunning = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - timeFrame);
        if (Attacking == true && player != null && !IsAttacked)
        {
            //caso ele possa atacar o jogador
            player.Attacked(damage);
            anim.SetBool("Punching", true);
            punchsound.Play();
            StartCoroutine(AttackEnemy());
            ATKrunning = false;
            yield break;
        }
        else
        {
            //caso o jogador fuja da frame do inimigo
            ATKrunning = false;
            anim.SetBool("Move", false);
            anim.SetBool("Punching", false);
        }

    }
}
