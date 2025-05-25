using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StressBarManager : MonoBehaviour
{
    [SerializeField] private Image StressBar;
    [SerializeField] private Animator animator;
    
    [SerializeField] private float maxStress = 100f;
    [SerializeField] public float actualStress = 0f;

    private float Stress10 = 10f;
    private float Stress30 = 30f;
    private float Stress50 = 50f;
    private float Stress70 = 70f;
    private float Stress90 = 90f;
    
    [SerializeField] private string changeScene = "FightClub";
    [SerializeField] private float gameOverTimer = 60f;
    
    private bool gameActive = false;
    private bool gameOverScheduled = false;

    private static readonly string PARAMETRO_NIVEL_STRESS = "Stress";

    void OnEnable()
    {
        CafeSceneManager.OnGameStarted += HandleGameStarted;
        CafeSceneManager.OnGameEnded += HandleGameEnded;
    }
    
    void OnDisable()
    {
        CafeSceneManager.OnGameStarted -= HandleGameStarted;
        CafeSceneManager.OnGameEnded -= HandleGameEnded;
    }
    
    void HandleGameStarted()
    {
        gameActive = true;
        Invoke("GameOver", gameOverTimer);
        gameOverScheduled = true;
    }
    
    void HandleGameEnded()
    {
        gameActive = false;
        if (gameOverScheduled)
        {
            CancelInvoke("GameOver");
            gameOverScheduled = false;
        }
    }

    private void Start()
    {
        Invoke("GameOver", gameOverTimer);
        UpdateBar();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void IncreaseStress(float quantity)
    {
        if (!gameActive) return;
        
        actualStress = Mathf.Clamp(actualStress + quantity-11, 0, maxStress);
        UpdateBar();
        UpdateAnimator();
    }
    
    public void DecreaseStress(float quantity)
    {
        if (!gameActive) return;
        
        actualStress = Mathf.Clamp(actualStress - quantity, 0, maxStress);
        UpdateBar();
        UpdateAnimator();
    }

    public void DefineStress(float value)
    {
        if (!gameActive) return;
        
        actualStress = Mathf.Clamp(value, 0, maxStress);
        UpdateBar();
        UpdateAnimator();
    }
    
    private void UpdateBar()
    {
        if (StressBar != null)
        {
            float percentage = actualStress / maxStress;
            StressBar.fillAmount = percentage;
        }
    }
    
    private void UpdateAnimator()
    {
        if (animator != null)
        {
            int stressInt = Mathf.RoundToInt((actualStress / maxStress) * 100f);
            animator.SetInteger(PARAMETRO_NIVEL_STRESS, stressInt);
        }
    }
    
    private void GameOver()
    {
        gameOverScheduled = false;
        
        if (!string.IsNullOrEmpty(changeScene))
        {
            SceneManager.LoadScene(changeScene);
        }
    }
    
    public float ActualStress()
    {
        return actualStress;
    }
    
    public float StressPercentage()
    {
        return actualStress / maxStress;
    }

    private void Update()
    {
        if (gameActive)
        {
            DefineStress(actualStress);
        }
    }
}