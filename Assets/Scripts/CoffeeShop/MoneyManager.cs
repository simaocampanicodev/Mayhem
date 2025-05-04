using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int currentMoney = 0;
    [SerializeField] private TextMeshProUGUI moneyText;
    
    public System.Action OnCorrectOrderSound;
    public System.Action OnIncorrectOrderSound;
    public System.Action OnSilhouetteTimeoutSound;

    private void Start()
    {
        UpdateMoneyUI();
    }
    
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
        if (amount > 0) OnCorrectOrderSound?.Invoke();
    }
    
    public void SubtractMoney(int amount)
    {
        currentMoney -= amount;
        if (currentMoney < 0) currentMoney = 0;

        UpdateMoneyUI();
        if (amount > 0) OnIncorrectOrderSound?.Invoke();
    }
    
    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"${currentMoney}";
        }
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }
}