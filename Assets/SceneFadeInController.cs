using UnityEngine;
using System.Collections;

public class DoctorFadeIn : MonoBehaviour
{
    public SpriteRenderer doctorSprite;
    [SerializeField] float fadeDuration = 2f;

    void Start()
    {
        Color c = doctorSprite.color;
        c.a = 0f;
        doctorSprite.color = c;
        
        Invoke("StartFade", 4.1f);
    }

    void StartFade()
    {
        StartCoroutine(FadeInDoctor());
    }

    IEnumerator FadeInDoctor()
    {
        float timer = 0f;
        Color c = doctorSprite.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            c.a = alpha;
            doctorSprite.color = c;
            yield return null;
        }
        
        c.a = 1f;
        doctorSprite.color = c;
    }
}