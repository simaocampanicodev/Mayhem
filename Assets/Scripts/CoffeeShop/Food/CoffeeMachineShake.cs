using UnityEngine;
using System.Collections;

public class CoffeeMachineShake : MonoBehaviour
{
    [SerializeField] private float shakeIntensity = 0.05f;
    [SerializeField] private float shakeFrequency = 10f;
    
    private Vector3 originalPosition;
    private bool isShaking = false;
    private Coroutine shakeCoroutine;
    
    private void Start()
    {
        originalPosition = transform.localPosition;
    }
    
    public void StartShaking()
    {
        if (isShaking) return;
        
        isShaking = true;
        originalPosition = transform.localPosition;
        
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
            
        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }
    
    public void StopShaking()
    {
        if (!isShaking) return;
        
        isShaking = false;
        
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
            
        transform.localPosition = originalPosition;
    }
    
    private IEnumerator ShakeRoutine()
    {
        while (isShaking)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * shakeIntensity;
            
            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
            
            yield return new WaitForSeconds(1f / shakeFrequency);
        }
        
        transform.localPosition = originalPosition;
    }
}