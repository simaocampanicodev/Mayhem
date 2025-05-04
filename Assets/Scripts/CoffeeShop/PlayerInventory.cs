using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private bool hasCoffee = false;
    [SerializeField] private bool hasToast = false;
    [SerializeField] private GameObject coffeeIndicator;
    [SerializeField] private GameObject toastIndicator;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float messageDuration = 2f;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audios;

    void Update()
    {
        
    }

    void Start()
    {
        UpdateUI();
        
        if (moneyManager == null)
        {
            moneyManager = FindObjectOfType<MoneyManager>();
        }
    }

    public bool HasCoffee()
    {
        return hasCoffee;
    }

    public bool HasToast()
    {
        return hasToast;
    }

    public void ToggleCoffee(bool value)
    {
        hasCoffee = value;
        UpdateUI();
    }

    public void ToggleToast(bool value)
    {
        hasToast = value;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coffeeIndicator != null)
            coffeeIndicator.SetActive(hasCoffee);

        if (toastIndicator != null)
            toastIndicator.SetActive(hasToast);
    }

    public bool CheckAndDeliverOrder(SilhouetteOrder.OrderType orderType)
    {
        bool isCorrect = false;
        
        switch (orderType)
        {
            case SilhouetteOrder.OrderType.Coffee:
                isCorrect = hasCoffee && !hasToast;
                break;

            case SilhouetteOrder.OrderType.Toast:
                isCorrect = hasToast && !hasCoffee;
                break;

            case SilhouetteOrder.OrderType.Both:
                isCorrect = hasToast && hasCoffee;
                break;
        }
        
        if (isCorrect)
        {
            audioSource.PlayOneShot(audios[0]);
            switch (orderType)
            {
                case SilhouetteOrder.OrderType.Coffee:
                    hasCoffee = false;
                    break;

                case SilhouetteOrder.OrderType.Toast:
                    hasToast = false;
                    break;

                case SilhouetteOrder.OrderType.Both:
                    hasCoffee = false;
                    hasToast = false;
                    break;
            }

            UpdateUI();
            
            moneyManager?.AddMoney(20);
        }
        else
        {
            audioSource.PlayOneShot(audios[1]);
            moneyManager?.SubtractMoney(10);
        }

        return isCorrect;
    }
}