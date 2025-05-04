using UnityEngine;
using System.Collections;
using TMPro;

public class FoodPreparation : MonoBehaviour
{
    [SerializeField] private float preparationTime = 2f;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject coffeeProgressIndicator;
    [SerializeField] private GameObject toastProgressIndicator;
    [SerializeField] private float messageDuration = 2f;
    [SerializeField] private bool showDebug = true;
    
    private bool coffeeReady = false;
    private bool toastReady = false;
    private bool isPreparingCoffee = false;
    private bool isPreparingToast = false;
    private bool playerInCoffeeArea = false;
    private bool playerInToastArea = false;
    private bool gameActive = false;
    
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
    }
    
    void HandleGameEnded()
    {
        gameActive = false;
    }
    
    public void SetPlayerInCoffeeArea(bool value)
    {
        playerInCoffeeArea = value;
    }
    
    public void SetPlayerInToastArea(bool value)
    {
        playerInToastArea = value;
    }

    private void Start()
    {
        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
        }
        if (coffeeProgressIndicator != null)
            coffeeProgressIndicator.SetActive(false);
        if (toastProgressIndicator != null)
            toastProgressIndicator.SetActive(false);
    }

    private void Update()
    {
        if (!gameActive) return;
        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (playerInCoffeeArea)
            {
                    
                if (!isPreparingCoffee && !coffeeReady)
                {
                    StartCoroutine(PrepareCoffee());
                }
                else if (coffeeReady)
                {
                    TryCollectCoffee();
                }
            }
            if (playerInToastArea)
            {
                    
                if (!isPreparingToast && !toastReady)
                {
                    StartCoroutine(PrepareToast());
                }
                else if (toastReady)
                {
                    TryCollectToast();
                }
            }
        }
    }

    private IEnumerator PrepareCoffee()
    {
        isPreparingCoffee = true;

        if (coffeeProgressIndicator != null)
            coffeeProgressIndicator.SetActive(true);

        yield return new WaitForSeconds(preparationTime);

        coffeeReady = true;
        isPreparingCoffee = false;
    }

    private IEnumerator PrepareToast()
    {
        isPreparingToast = true;

        if (toastProgressIndicator != null)
            toastProgressIndicator.SetActive(true);

        yield return new WaitForSeconds(preparationTime);

        toastReady = true;
        isPreparingToast = false;
    }

    private void TryCollectCoffee()
    {
        bool hasCoffee = playerInventory.HasCoffee();
        
        if (!hasCoffee)
        {
            playerInventory.ToggleCoffee(true);
            coffeeReady = false;
            
            if (coffeeProgressIndicator != null)
                coffeeProgressIndicator.SetActive(false);
        }
    }

    private void TryCollectToast()
    {
        bool hasToast = playerInventory.HasToast();
        
        if (!hasToast)
        {
            playerInventory.ToggleToast(true);
            toastReady = false;
            
            if (toastProgressIndicator != null)
                toastProgressIndicator.SetActive(false);
        }
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
            
            CancelInvoke("HideMessage");
            Invoke("HideMessage", messageDuration);
        }
    }
    
    private void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }
}