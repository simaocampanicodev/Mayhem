using UnityEngine;
using System.Collections;

public class NPCManager : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public float difficultyMultiplier = 1f;

    private GameObject currentNPC;

    void Start()
    {
        StartCoroutine(SpawnCustomer());
        StartCoroutine(IncreaseDifficultyOverTime());
    }

    private IEnumerator SpawnCustomer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4, 8));

            if (currentNPC == null)
            {
                currentNPC = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
                NPCOrder npcOrder = currentNPC.GetComponent<NPCOrder>();
                npcOrder.GenerateRandomOrder(difficultyMultiplier);
            }
        }
    }

    private IEnumerator IncreaseDifficultyOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(30);
            difficultyMultiplier += 0.2f;
            Debug.Log("Dificuldade aumentou! Multiplicador: " + difficultyMultiplier);
        }
    }

    public void RemoveCustomer()
    {
        if (currentNPC != null)
        {
            Destroy(currentNPC);
            currentNPC = null;
        }
    }
}
