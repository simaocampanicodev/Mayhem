using UnityEngine;
using System.Collections;

public class NPCManager : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public float difficultyMultiplier = 1f;

    private GameObject leftNPC;
    private GameObject rightNPC;
    private bool isSpawning = false;

    void Start()
    {
        // Inicializar os NPCs com pedidos específicos
        SpawnNPCs();
    }

    private void SpawnNPCs()
{
    if (isSpawning) return;
    isSpawning = true;

    // Gera pedidos aleatórios para os NPCs
    NPCOrder.OrderType leftOrderType = (NPCOrder.OrderType)Random.Range(0, 3);
    NPCOrder.OrderType rightOrderType = (NPCOrder.OrderType)Random.Range(0, 3);

    // Garantir que os pedidos sejam diferentes
    while (leftOrderType == rightOrderType)
    {
        rightOrderType = (NPCOrder.OrderType)Random.Range(0, 3);
    }

    // Spawn NPC da esquerda
    if (leftNPC == null && leftSpawnPoint != null)
    {
        leftNPC = Instantiate(npcPrefab, leftSpawnPoint.position, Quaternion.identity);
        NPCOrder leftOrder = leftNPC.GetComponent<NPCOrder>();
        leftOrder.currentOrder = leftOrderType;
        Debug.Log("NPC da esquerda criado com pedido: " + leftOrderType);
    }

    // Spawn NPC da direita
    if (rightNPC == null && rightSpawnPoint != null)
    {
        rightNPC = Instantiate(npcPrefab, rightSpawnPoint.position, Quaternion.identity);
        NPCOrder rightOrder = rightNPC.GetComponent<NPCOrder>();
        rightOrder.currentOrder = rightOrderType;
        Debug.Log("NPC da direita criado com pedido: " + rightOrderType);
    }

    isSpawning = false;

    // Atualize a UI de pedidos
    OrderPanelManager.Instance.UpdateOrders();
}


    // private IEnumerator SpawnCustomer()
    //  {
    //  while (true)
    //   {
    //      yield return new WaitForSeconds(Random.Range(4, 8));

    //     if (currentNPC == null)
    //      {
    //         currentNPC = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
    //          NPCOrder npcOrder = currentNPC.GetComponent<NPCOrder>();
    //         npcOrder.GenerateRandomOrder(difficultyMultiplier);
    //     }
    //   }
    // }

    //private IEnumerator IncreaseDifficultyOverTime()
    //{
    //while (true)
    //{
    // yield return new WaitForSeconds(30);
    //  difficultyMultiplier += 0.2f;
    //    Debug.Log("Dificuldade aumentou! Multiplicador: " + difficultyMultiplier);
    //  }
    //  }

    public void RemoveCustomer()
    {
        if (leftNPC)
        {
            Destroy(leftNPC);
            Debug.Log("NPC da esquerda removido");
        }

        if (rightNPC)
        {
            Destroy(rightNPC);
            Debug.Log("NPC da direita removido");
        }

        // Criar novos NPCs após uma pequena espera
        Invoke(nameof(SpawnNPCs), 2f);

        // Atualize os pedidos na UI
        OrderPanelManager.Instance.UpdateOrders();
    }


}
