using UnityEngine;

public class CoffeeArea : MonoBehaviour
{
    [SerializeField] private FoodPreparation foodPrep;

    private void Start()
    {
        if (foodPrep == null)
        {
            foodPrep = FindFirstObjectByType<FoodPreparation>();

        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && !collider.isTrigger)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foodPrep.SetPlayerInCoffeeArea(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foodPrep.SetPlayerInCoffeeArea(false);
        }
    }
}
