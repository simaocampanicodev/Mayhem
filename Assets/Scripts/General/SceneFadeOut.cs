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
    [SerializeField] private bool HasRequirements = false;

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
        if (LoadNextScene)
        {
            if (HasRequirements)
            {
                PlayerScript plr = FindFirstObjectByType<PlayerScript>().GetComponent<PlayerScript>();
                if (plr.BeatenEnemies > 5)
                {
                    SceneManager.LoadSceneAsync(sceneName);
                }
                else
                {
                    SceneManager.LoadSceneAsync("TitleScreen");
                }
            }
            else { SceneManager.LoadSceneAsync(sceneName); }
        }
        gameObject.SetActive(false);
    }
}