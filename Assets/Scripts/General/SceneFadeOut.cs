using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFadeOut : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 4f;
    [SerializeField] private bool LoadNextScene = false;
    [SerializeField] private string sceneName;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);

        Color c = fadeImage.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        fadeImage.color = c;
        if (LoadNextScene) { SceneManager.LoadSceneAsync(sceneName); }
        gameObject.SetActive(false);
    }
}