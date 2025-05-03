using UnityEngine;
using System.Collections;

public class ParticleTimer : MonoBehaviour
{
    [SerializeField] private float time = .1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroySelf(time));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroySelf(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
