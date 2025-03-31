using UnityEngine;
using System.Collections;

public class NPCOrder : MonoBehaviour
{
    public enum OrderType { Coffee, Toast, Both }
    public OrderType currentOrder;

    private float orderTime;
    private float maxOrderTime = 15f;
    private bool orderActive = false;

    private NPCOrderUI orderUI;

    private void Start()
    {
        orderUI = FindFirstObjectByType<NPCOrderUI>();
        GenerateNewOrder();
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
        Debug.Log($"Pedido falhou para NPC {gameObject.name}! Perdeu dinheiro.");
        FindFirstObjectByType<GameManager>().RemoveSleep(200);

        GenerateNewOrder();
    }

    private void GenerateNewOrder()
    {
        StopAllCoroutines();
        currentOrder = (OrderType)Random.Range(0, 3); // pedido aleatorio
        orderTime = maxOrderTime;
        orderActive = true;

        Debug.Log($"Novo pedido para {gameObject.name}: {currentOrder}");

        orderUI.UpdateOrder(currentOrder);


        StartCoroutine(OrderCountdown()); 
    }

    public float GetRemainingTime()
    {
        return orderTime;
    }
}
