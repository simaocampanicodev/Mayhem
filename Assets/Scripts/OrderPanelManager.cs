using UnityEngine;
using TMPro;

public class OrderPanelManager : MonoBehaviour
{
    public static OrderPanelManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI orderText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateOrders();
    }

    public void UpdateOrders()
    {
        if (orderText == null)
        {
            Debug.LogError("orderText não está atribuído!");
            return;
        }

        // Encontrar todos os NPCs na cena usando o novo método recomendado
        NPCOrder[] npcOrders = FindObjectsByType<NPCOrder>(FindObjectsSortMode.None);

        string displayText = "Pedidos Ativos:\n";
        for (int i = 0; i < npcOrders.Length; i++)
        {
            if (npcOrders[i] != null)
            {
                string npcName = (i == 0) ? "NPC 1 (E)" : "NPC 2 (D)";
                string orderTypeText = GetOrderTypeText(npcOrders[i].currentOrder);
                displayText += $"• {npcName}: {orderTypeText}\n";
            }
        }

        orderText.text = displayText;
    }

    private string GetOrderTypeText(NPCOrder.OrderType orderType)
    {
        switch (orderType)
        {
            case NPCOrder.OrderType.Coffee:
                return "☕ Café";
            case NPCOrder.OrderType.Toast:
                return "🍞 Torrada";
            case NPCOrder.OrderType.Both:
                return "☕+🍞 Café e Torrada";
            default:
                return "Nenhum";
        }
    }
}