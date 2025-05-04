using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalLevel : MonoBehaviour
{
    [SerializeField] private Image black;
    [SerializeField] private float fadeDuration;
    [SerializeField] private int num;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player") {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        
        Color c = black.color;
        float timer = fadeDuration;

        while (timer < fadeDuration)
        {
            timer -= Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            black.color = c;
            yield return null;
        }

        c.a = 0f;
        black.color = c;
        SceneManager.LoadSceneAsync(num);
    }
}
