using UnityEngine;

public class ToastArea : MonoBehaviour
{
    [SerializeField] private FoodPreparation foodPrep;
    [SerializeField] private bool showDebug = true;

    private void Start()
    {
        if (foodPrep == null)
        {
            foodPrep = FindObjectOfType<FoodPreparation>();
            if (foodPrep == null && showDebug)
                Debug.LogError("FoodPreparation não encontrado!");
        }
        
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && !collider.isTrigger)
        {
            collider.isTrigger = true;
            if (showDebug)
                Debug.Log("Collider configurado como trigger.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (showDebug)
                Debug.Log("Jogador entrou na área de tosta");
                
            foodPrep.SetPlayerInToastArea(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (showDebug)
                Debug.Log("Jogador saiu da área de tosta");
                
            foodPrep.SetPlayerInToastArea(false);
        }
    }
}