using UnityEngine;
using TMPro;

public class NPCOrderUI : MonoBehaviour
{
    public TextMeshProUGUI orderText;

    public void UpdateOrder(NPCOrder.OrderType orderType)
    {
        switch (orderType)
        {
            case NPCOrder.OrderType.Coffee:
                orderText.text = "Pedido: \U00002615 Café";
                break;
            case NPCOrder.OrderType.Toast:
                orderText.text = "Pedido: \U0001F35E Torrada";
                break;
            case NPCOrder.OrderType.Both:
                orderText.text = "Pedido: \U00002615+\U0001F35E Café e Torrada";
                break;
            default:
                orderText.text = "Pedido: Nenhum";
                break;
        }
    }

    public void HideOrder()
    {
        orderText.text = "";
    }
}
