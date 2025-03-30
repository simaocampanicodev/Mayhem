using UnityEngine;
using System.Collections;

public class NPCOrder : MonoBehaviour
{
    public enum OrderType { Coffee, Toast, Both }
    public OrderType currentOrder;

    private float orderTime;
    private float maxOrderTime = 15f;
    private bool orderActive = false;

    private void Start()
    {
        orderTime = maxOrderTime;
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
        Debug.Log($"Pedido falhou para NPC {gameObject.name}! Perdeu dinheiro.");
        FindFirstObjectByType<NPCManager>().RemoveCustomer();
        FindFirstObjectByType<GameManager>().RemoveSleep(200);
    }

    public float GetRemainingTime()
    {
        return orderTime;
    }
}
