using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI    MoneyText;

    public void UpdateMoney(int money)
    {
        if (MoneyText != null)
        {
            MoneyText.text = "Dinheiro: " + money.ToString();
        }
    }
}
