using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private bool hasCoffee = false;
    [SerializeField] private bool hasToast = false;
    [SerializeField] private bool hasColdCoffee = false;
    [SerializeField] private bool hasColdToast = false;

    [SerializeField] private GameObject coffeeIndicator;
    [SerializeField] private GameObject toastIndicator;
    [SerializeField] private GameObject coldCoffeeIndicator;
    [SerializeField] private GameObject coldToastIndicator;

    [SerializeField] private float messageDuration = 2f;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audios;
    [SerializeField] private StressBarManager stressManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (hasToast)
            {
                DropToast();
            }
            else if (hasColdToast)
            {
                DropColdToast();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hasCoffee)
            {
                DropCoffee();
            }
            else if (hasColdCoffee)
            {
                DropColdCoffee();
            }
        }
    }

    void Start()
    {
        UpdateUI();

        if (moneyManager == null)
        {
            moneyManager = FindObjectOfType<MoneyManager>();
        }
        if (stressManager == null)
        {
            stressManager = FindObjectOfType<StressBarManager>();
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

    public bool HasColdCoffee()
    {
        return hasColdCoffee;
    }

    public bool HasColdToast()
    {
        return hasColdToast;
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

    public void ToggleColdCoffee(bool value)
    {
        hasColdCoffee = value;
        UpdateUI();
    }

    public void ToggleColdToast(bool value)
    {
        hasColdToast = value;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coffeeIndicator != null)
            coffeeIndicator.SetActive(hasCoffee);

        if (toastIndicator != null)
            toastIndicator.SetActive(hasToast);

        if (coldCoffeeIndicator != null)
            coldCoffeeIndicator.SetActive(hasColdCoffee);

        if (coldToastIndicator != null)
            coldToastIndicator.SetActive(hasColdToast);
    }

    private void DropToast()
    {
        hasToast = false;
        UpdateUI();
    }

    private void DropCoffee()
    {
        hasCoffee = false;
        UpdateUI();
    }

    private void DropColdToast()
    {
        hasColdToast = false;
        UpdateUI();
    }

    private void DropColdCoffee()
    {
        hasColdCoffee = false;
        UpdateUI();
    }

    public bool CheckAndDeliverOrder(SilhouetteOrder.OrderType orderType)
    {
        bool isCorrect = false;
        bool hasWrongColdItems = false;

        switch (orderType)
        {
            case SilhouetteOrder.OrderType.Coffee:
                isCorrect = hasCoffee;
                hasWrongColdItems = hasColdCoffee;
                break;

            case SilhouetteOrder.OrderType.Toast:
                isCorrect = hasToast;
                hasWrongColdItems = hasColdToast;
                break;

            case SilhouetteOrder.OrderType.Both:
                isCorrect = hasToast && hasCoffee;
                hasWrongColdItems = hasColdToast || hasColdCoffee;
                break;
        }

        if (hasWrongColdItems)
        {
            audioSource.PlayOneShot(audios[1]);
            moneyManager?.SubtractMoney(10);
            if (stressManager != null)
                stressManager.IncreaseStress(20f);

            if (orderType == SilhouetteOrder.OrderType.Coffee && hasColdCoffee)
            {
                hasColdCoffee = false;
            }
            else if (orderType == SilhouetteOrder.OrderType.Toast && hasColdToast)
            {
                hasColdToast = false;
            }
            else if (orderType == SilhouetteOrder.OrderType.Both)
            {
                if (hasColdCoffee) hasColdCoffee = false;
                if (hasColdToast) hasColdToast = false;
            }

            UpdateUI();
            return false;
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
            return true;
        }
        else
        {
            audioSource.PlayOneShot(audios[1]);
            moneyManager?.SubtractMoney(10);
            if (stressManager != null)
                stressManager.IncreaseStress(20f);
            return false;
        }
    }
}