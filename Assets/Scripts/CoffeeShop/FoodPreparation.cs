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
    
    public void SetPlayerInCoffeeArea(bool value)
    {
        playerInCoffeeArea = value;
        if (showDebug)
            Debug.Log("Player in coffee area: " + value);
    }
    
    public void SetPlayerInToastArea(bool value)
    {
        playerInToastArea = value;
        if (showDebug)
            Debug.Log("Player in toast area: " + value);
    }

    private void Start()
    {
        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            if (playerInventory == null && showDebug)
                Debug.LogError("PlayerInventory não encontrado!");
        }
        if (coffeeProgressIndicator != null)
            coffeeProgressIndicator.SetActive(false);
        if (toastProgressIndicator != null)
            toastProgressIndicator.SetActive(false);
            
        if (showDebug)
            Debug.Log("FoodPreparation iniciado. Pressione Enter nas áreas corretas para preparar comida.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (showDebug)
                Debug.Log("Tecla Enter pressionada");
            
            if (playerInCoffeeArea)
            {
                if (showDebug)
                    Debug.Log("Jogador está na área de café");
                    
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
                if (showDebug)
                    Debug.Log("Jogador está na área de tosta");
                    
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
        ShowMessage("Preparando café...");
        Debug.Log("Preparando café...");

        if (coffeeProgressIndicator != null)
            coffeeProgressIndicator.SetActive(true);

        yield return new WaitForSeconds(preparationTime);

        coffeeReady = true;
        isPreparingCoffee = false;
        ShowMessage("Café pronto!");
        Debug.Log("Café pronto!");
    }

    private IEnumerator PrepareToast()
    {
        isPreparingToast = true;
        ShowMessage("Preparando tosta...");
        Debug.Log("Preparando tosta...");

        if (toastProgressIndicator != null)
            toastProgressIndicator.SetActive(true);

        yield return new WaitForSeconds(preparationTime);

        toastReady = true;
        isPreparingToast = false;
        ShowMessage("Tosta pronta!");
        Debug.Log("Tosta pronta!");
    }

    private void TryCollectCoffee()
    {
        bool hasCoffee = playerInventory.HasCoffee();
        
        if (!hasCoffee)
        {
            playerInventory.ToggleCoffee(true);
            coffeeReady = false;
            ShowMessage("Pegou café");
            Debug.Log("Pegou café");
            
            if (coffeeProgressIndicator != null)
                coffeeProgressIndicator.SetActive(false);
        }
        else
        {
            ShowMessage("Você já tem café!");
            Debug.Log("Você já tem café!");
        }
    }

    private void TryCollectToast()
    {
        bool hasToast = playerInventory.HasToast();
        
        if (!hasToast)
        {
            playerInventory.ToggleToast(true);
            toastReady = false;
            ShowMessage("Pegou tosta");
            Debug.Log("Pegou tosta");
            
            if (toastProgressIndicator != null)
                toastProgressIndicator.SetActive(false);
        }
        else
        {
            ShowMessage("Você já tem tosta!");
            Debug.Log("Você já tem tosta!");
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