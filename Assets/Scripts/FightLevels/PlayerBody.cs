using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBody : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GruntScript hurtSound;
    private SpriteRenderer spr;
    [SerializeField] private bool PlayerBod;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerBod) { StartCoroutine(LoseTimer()); }
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
        while (spr.color.a > 0f)
        {
            Color c = spr.color;
            c.a = Mathf.Max(0f, c.a - 0.1f);
            spr.color = c;
            yield return new WaitForSeconds(1f);
        }
        Destroy(gameObject);
    }

    IEnumerator LoseTimer()
    {
        Image fadeImage = GameObject.Find("Black").GetComponent<Image>();
        yield return new WaitForSeconds(6f);
        Color c = fadeImage.color;
        float timer = 0f;

        while (timer < 4)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / 4);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadSceneAsync("TitleScreen",LoadSceneMode.Single);
    }
}
