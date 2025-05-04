using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SilhouetteManager : MonoBehaviour
{
    [SerializeField] private GameObject silhouettePrefab;
    [SerializeField] private Transform[] seatPositions;
    [SerializeField] private float minStartDelay = 1f;
    [SerializeField] private float maxStartDelay = 5f;
    [SerializeField] private float silhouetteDuration = 15f;
    [SerializeField] private int maxSilhouettes = 2;
    [SerializeField] private float minDelayAtMaxStress = 1f;
    [SerializeField] private float maxDelayAtMaxStress = 2f;
    [SerializeField] private StressBarManager stressManager;
    [SerializeField] private OrderDisplay orderDisplay;

    private List<int> occupiedSeats = new List<int>();
    private bool gameStarted = false;

    void OnEnable()
    {
        CafeSceneManager.OnGameStarted += HandleGameStarted;
    }
    
    void OnDisable()
    {
        CafeSceneManager.OnGameStarted -= HandleGameStarted;
    }
    
    void HandleGameStarted()
    {
        gameStarted = true;
        StartSpawning();
    }

    void Start()
    {
        if (stressManager == null)
        {
            stressManager = FindObjectOfType<StressBarManager>();
        }
        
        if (orderDisplay == null)
        {
            orderDisplay = FindObjectOfType<OrderDisplay>();
        }
    }
    
    void StartSpawning()
    {
        for (int i = 0; i < maxSilhouettes; i++)
        {
            float delay = GetRandomDelay();
            StartCoroutine(SpawnSilhouetteWithDelay(delay));
        }
    }

    float GetRandomDelay()
    {
        float stressPercent = stressManager != null ? stressManager.StressPercentage() : 0f;
        float minDelay = Mathf.Lerp(minStartDelay, minDelayAtMaxStress, stressPercent);
        float maxDelay = Mathf.Lerp(maxStartDelay, maxDelayAtMaxStress, stressPercent);
        
        return Random.Range(minDelay, maxDelay);
    }

    IEnumerator SpawnSilhouetteWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnSilhouette();
    }

    void SpawnSilhouette()
    {
        if (occupiedSeats.Count >= maxSilhouettes) return;

        int seatIndex = GetRandomFreeSeat();
        if (seatIndex == -1) return;

        occupiedSeats.Add(seatIndex);

        GameObject silhouette = Instantiate(silhouettePrefab, seatPositions[seatIndex].position, Quaternion.identity, transform);
        SilhouetteOrder s = silhouette.GetComponent<SilhouetteOrder>();

        if (s != null)
        {
            s.Initialize(silhouetteDuration - 3f, () => {
                if (orderDisplay != null)
                {
                    orderDisplay.HideOrder(seatIndex);
                }
                
                occupiedSeats.Remove(seatIndex);
                float delay = GetRandomDelay();
                StartCoroutine(SpawnSilhouetteWithDelay(delay));
                
                if (stressManager != null && !s.IsOrderFulfilled())
                {
                    stressManager.IncreaseStress(15f);
                    GetComponent<MoneyManager>()?.SubtractMoney(15);
                    GetComponent<MoneyManager>()?.OnSilhouetteTimeoutSound?.Invoke();
                }
            });
            
            if (orderDisplay != null)
            {
                orderDisplay.ShowOrder(seatIndex, s.GetCurrentOrder());
            }
        }
    }

    int GetRandomFreeSeat()
    {
        List<int> freeSeats = new List<int>();
        for (int i = 0; i < seatPositions.Length; i++)
        {
            if (!occupiedSeats.Contains(i))
                freeSeats.Add(i);
        }

        if (freeSeats.Count == 0) return -1;

        return freeSeats[Random.Range(0, freeSeats.Count)];
    }
}