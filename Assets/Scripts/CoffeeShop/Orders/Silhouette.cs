using UnityEngine;
using System.Collections;

public class Silhouette : MonoBehaviour
{
    [SerializeField] float fadeDuration = 1.5f;

    public void Initialize(float visibleDuration, System.Action onFinished)
    {
        StartCoroutine(FadeInOutSequence(visibleDuration, onFinished));
    }

    private IEnumerator FadeInOutSequence(float visibleTime, System.Action onFinished)
    {
        yield return Fade(0, 1, fadeDuration);
        
        yield return new WaitForSeconds(visibleTime);
        
        yield return Fade(1, 0, fadeDuration);

        onFinished?.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        float elapsed = 0;
        Color color = sr.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            sr.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(color.r, color.g, color.b, to);
    }

}