using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CafeSceneManager : MonoBehaviour
{
    public Image fadePanel;
    public float totalTime = 60f;
    public float fadeDuration = 5f;
    private float fullBlackDuration = 0.4f;

    [SerializeField] private SilhouetteManager silhouetteManager;
    [SerializeField] private StressBarManager stressManager;
    [SerializeField] private FoodPreparation foodPreparation;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioSource audioSource;
    public delegate void GameStateChanged();
    public static event GameStateChanged OnGameStarted;
    public static event GameStateChanged OnGameEnded;
    
    private bool gameActive = false;

    void Awake()
    {
        if (silhouetteManager == null)
            silhouetteManager = FindObjectOfType<SilhouetteManager>();
            
        if (stressManager == null)
            stressManager = FindObjectOfType<StressBarManager>();
            
        if (foodPreparation == null)
            foodPreparation = FindObjectOfType<FoodPreparation>();
        
        if (silhouetteManager != null)
            silhouetteManager.enabled = false;
            
        if (stressManager != null)
            stressManager.enabled = false;
            
        if (foodPreparation != null)
            foodPreparation.enabled = false;
    }

    void Start()
    {
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        if (fadePanel != null)
        {
            Color panelColor = fadePanel.color;
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, 1f);
        }
        StartCoroutine(GameSequence());
    }

    IEnumerator GameSequence()
    {
        yield return StartCoroutine(FadeFromBlack());
        
        ActivateGameSystems();
        
        yield return new WaitForSeconds(totalTime - fadeDuration - fullBlackDuration);
        
        yield return StartCoroutine(FadeToBlack());
        
        yield return new WaitForSeconds(fullBlackDuration);
        
        if (OnGameEnded != null)
            OnGameEnded();
    }

    IEnumerator FadeFromBlack()
    {
        float time = 0f;
        Color panelColor = fadePanel.color;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, 0f);
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
    
    private void ActivateGameSystems()
    {
        if (silhouetteManager != null)
            silhouetteManager.enabled = true;
            
        if (stressManager != null)
            stressManager.enabled = true;
            
        if (foodPreparation != null)
            foodPreparation.enabled = true;
        
        gameActive = true;
        if (OnGameStarted != null)
            OnGameStarted();
    }
}