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
    [SerializeField] private AudioClip correctOrderSound;
    [SerializeField] private AudioClip incorrectOrderSound;
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
            switch (orderType)
            {
                case SilhouetteOrder.OrderType.Coffee:
                    audioSource.PlayOneShot(audios[0]);
                    hasCoffee = false;
                    break;

                case SilhouetteOrder.OrderType.Toast:
                    audioSource.PlayOneShot(audios[1]);
                    hasToast = false;
                    break;

                case SilhouetteOrder.OrderType.Both:
                    audioSource.PlayOneShot(audios[1]);
                    audioSource.PlayOneShot(audios[0]);
                    hasCoffee = false;
                    hasToast = false;
                    break;
            }

            UpdateUI();
            
            moneyManager?.AddMoney(20);
            
            if (audioSource != null && correctOrderSound != null)
            {
                audioSource.PlayOneShot(correctOrderSound);
            }
        }
        else
        {
            moneyManager?.SubtractMoney(10);
            
            if (audioSource != null && incorrectOrderSound != null)
            {
                audioSource.PlayOneShot(incorrectOrderSound);
            }
        }

        return isCorrect;
    }
}