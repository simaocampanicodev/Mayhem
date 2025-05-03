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

    private List<int> occupiedSeats = new List<int>();

    void Start()
    {
        for (int i = 0; i < maxSilhouettes; i++)
        {
            float delay = Random.Range(minStartDelay, maxStartDelay);
            StartCoroutine(SpawnSilhouetteWithDelay(delay));
        }
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
        Silhouette s = silhouette.GetComponent<Silhouette>();

        s.Initialize(silhouetteDuration, () => {
            occupiedSeats.Remove(seatIndex);
            float delay = Random.Range(minStartDelay, maxStartDelay);
            StartCoroutine(SpawnSilhouetteWithDelay(delay));
        });
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