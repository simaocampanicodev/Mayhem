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

    void Update()
    {
       
    }
    
    void Start()
    {
        UpdateUI();
        
        if (messageText != null)
        {
            messageText.text = "";
            messageText.gameObject.SetActive(false);
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
    
    void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
            
            CancelInvoke("HideMessage");
            Invoke("HideMessage", messageDuration);
        }
    }
    
    void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
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
        }
        
        return isCorrect;
    }
}