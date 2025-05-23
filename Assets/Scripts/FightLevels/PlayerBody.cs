using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GruntScript hurtSound;
    private SpriteRenderer spr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spr = gameObject.GetComponent<SpriteRenderer>();
        AudioSource radioSource = gameObject.AddComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * 15, ForceMode2D.Impulse);
        AudioClip grunt = hurtSound.DeathSound;
        radioSource.PlayOneShot(grunt);
        StartCoroutine(FadeBody());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FadeBody()
    {
        while (spr.color.a > 0f) {
            Color c = spr.color;
            c.a = Mathf.Max(0f, c.a - 0.1f);
            spr.color = c;
            yield return new WaitForSeconds(1f);
        }
        Destroy(gameObject);
    }
}
