using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HospitalFadeIn : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 4f;

    void Start()
    {
        StartCoroutine(HandleSceneTransition());
    }

    IEnumerator HandleSceneTransition()
    {
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        yield return StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1f);

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
}