using UnityEngine;
using System.Collections;
using System.Data.SqlTypes;

public class NPCOrder : MonoBehaviour
{
    public enum OrderType { Coffee, Toast, Both }
    public OrderType currentOrder;

    private float orderTime;
    private float maxOrderTime = 15f;
    private bool orderActive = false;

    public void GenerateRandomOrder(float difficultyMultiplier)
    {
        currentOrder = (OrderType)Random.Range(0, 3);
        orderTime = maxOrderTime / difficultyMultiplier;
        orderActive = true;
        StartCoroutine(OrderCountdown());
    }

    private IEnumerator OrderCountdown()
    {
        while (orderActive && orderTime > 0)
        {
            yield return new WaitForSeconds(1f);
            orderTime--;
        }

        if (orderTime <= 0)
        {
            OrderFailed();
        }
    }

    public void OrderFailed()
    {
        Debug.Log("Pedido falhou! Perdeu dinheiro.");
        FindFirstObjectByType<GameManager>().RemoveSleep(-10);
        FindFirstObjectByType<NPCManager>().RemoveCustomer();
    }

    public float GetRemainingTime()
    {
        return orderTime;
    }
}
