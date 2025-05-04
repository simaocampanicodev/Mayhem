using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CafeSceneManager : MonoBehaviour
{
    public Image fadePanel;
    public float totalTime = 60f;
    private float fadeDuration = 5f;
    private float fullBlackDuration = 0.4f;

    void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(totalTime - fadeDuration - fullBlackDuration);
        yield return StartCoroutine(FadeToBlack());

        yield return new WaitForSeconds(fullBlackDuration);
    }

    IEnumerator FadeToBlack()
    {
        float time = 0f;
        Color panelColor = fadePanel.color;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, 1f);
    }
}