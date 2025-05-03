using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StressBarManager : MonoBehaviour
{
    [SerializeField] private Image StressBar;
    [SerializeField] private Animator animator;
    
    [SerializeField] private float maxStress = 100f;
    [SerializeField] private float actualStress = 0f;

    private float Stress10 = 10f;
    private float Stress30 = 30f;
    private float Stress50 = 50f;
    private float Stress70 = 70f;
    private float Stress90 = 90f;
    
    [SerializeField] private string changeScene = "Video";

    private static readonly string PARAMETRO_NIVEL_STRESS = "Stress";

    private void Start()
    {
        UpdateBar();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        Invoke("GameOver", 60f);
    }

    public void IncreaseStress(float quantity)
    {
        actualStress = Mathf.Clamp(actualStress + quantity, 0, maxStress);
        UpdateBar();
        UpdateAnimator();
    }
    
    public void DecreaseStress(float quantity)
    {
        actualStress = Mathf.Clamp(actualStress - quantity, 0, maxStress);
        UpdateBar();
        UpdateAnimator();
    }

    public void DefineStress(float value)
    {
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
        if (!string.IsNullOrEmpty(changeScene))
        {
            SceneManager.LoadScene(changeScene);
        }
        else
        {
            Debug.LogWarning("Nome da cena de Game Over não foi definido!");
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
        DefineStress(actualStress);
    }
}