using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PapersSceneTransition : MonoBehaviour
{
    public Image fadeImage;
    [SerializeField] public float fadeDuration = 2f;
    [SerializeField] private float totalSceneDuration = 8f;

    void Start()
    {
        StartCoroutine(HandlePapersTransition());
    }

    IEnumerator HandlePapersTransition()
    {
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        yield return StartCoroutine(FadeIn());

        float waitTime = totalSceneDuration - (fadeDuration * 2);
        yield return new WaitForSeconds(waitTime);
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        Color c = fadeImage.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 0f;
        fadeImage.color = c;
    }

    IEnumerator FadeOut()
    {
        Color c = fadeImage.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1f;
        fadeImage.color = c;
    }
}