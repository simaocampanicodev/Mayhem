using UnityEngine;
using System.Collections;

public class OrderDisplay : MonoBehaviour
{
    [SerializeField] private GameObject orderBackgroundPrefab;
    [SerializeField] private GameObject coffeeIconPrefab;
    [SerializeField] private GameObject toastIconPrefab;
    [SerializeField] private GameObject bothIconPrefab;
    [SerializeField] private Transform[] displayPositions;
    [SerializeField] private float fadeDuration = 0.3f;
    
    private GameObject[] activeOrderBackgrounds;
    private GameObject[] activeCoffeeIcons;
    private GameObject[] activeToastIcons;
    private GameObject[] activeBothIcons;
    
    private Coroutine[] fadeOutCoroutines;
    
    void Start()
    {
        fadeOutCoroutines = new Coroutine[displayPositions.Length];
        
        int numPositions = displayPositions.Length;
        activeOrderBackgrounds = new GameObject[numPositions];
        activeCoffeeIcons = new GameObject[numPositions];
        activeToastIcons = new GameObject[numPositions];
        activeBothIcons = new GameObject[numPositions];
    }
    
    public void ShowOrder(int positionIndex, SilhouetteOrder.OrderType orderType)
    {
        if (positionIndex < 0 || positionIndex >= displayPositions.Length)
        {
            return;
        }

        if (fadeOutCoroutines[positionIndex] != null)
        {
            StopCoroutine(fadeOutCoroutines[positionIndex]);
            fadeOutCoroutines[positionIndex] = null;
        }
        
        ClearOrder(positionIndex);
        
        Transform displayPos = displayPositions[positionIndex];
        activeOrderBackgrounds[positionIndex] = Instantiate(orderBackgroundPrefab, displayPos.position, Quaternion.identity, displayPos);
        
        switch (orderType)
        {
            case SilhouetteOrder.OrderType.Coffee:
                activeCoffeeIcons[positionIndex] = Instantiate(coffeeIconPrefab, displayPos.position, Quaternion.identity, displayPos);
                break;
                
            case SilhouetteOrder.OrderType.Toast:
                activeToastIcons[positionIndex] = Instantiate(toastIconPrefab, displayPos.position, Quaternion.identity, displayPos);
                break;
                
            case SilhouetteOrder.OrderType.Both:
                activeBothIcons[positionIndex] = Instantiate(bothIconPrefab, displayPos.position, Quaternion.identity, displayPos);
                break;
        }
        StartCoroutine(FadeOrderIn(positionIndex));
    }
    
    public void HideOrder(int positionIndex)
    {
        if (positionIndex < 0 || positionIndex >= displayPositions.Length)
            return;
        if (fadeOutCoroutines[positionIndex] != null)
        {
            StopCoroutine(fadeOutCoroutines[positionIndex]);
            fadeOutCoroutines[positionIndex] = null;
        }
            
        ClearOrder(positionIndex);
    }
    
    private void ClearOrder(int positionIndex)
    {
        if (activeOrderBackgrounds[positionIndex] != null)
        {
            Destroy(activeOrderBackgrounds[positionIndex]);
            activeOrderBackgrounds[positionIndex] = null;
        }
        
        if (activeCoffeeIcons[positionIndex] != null)
        {
            Destroy(activeCoffeeIcons[positionIndex]);
            activeCoffeeIcons[positionIndex] = null;
        }
        
        if (activeToastIcons[positionIndex] != null)
        {
            Destroy(activeToastIcons[positionIndex]);
            activeToastIcons[positionIndex] = null;
        }

        if (activeBothIcons[positionIndex] != null)
        {
            Destroy(activeBothIcons[positionIndex]);
            activeBothIcons[positionIndex] = null;
        }
    }
    
    private IEnumerator FadeOrderIn(int positionIndex)
    {
        SetOrderAlpha(positionIndex, 0f);
        
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            SetOrderAlpha(positionIndex, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        SetOrderAlpha(positionIndex, 1f);
    }
    
    private IEnumerator FadeOrderOut(int positionIndex)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            SetOrderAlpha(positionIndex, 1f - (elapsed / fadeDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        ClearOrder(positionIndex);
    }
    
    private void SetOrderAlpha(int positionIndex, float alpha)
    {
        SetSpriteAlpha(activeOrderBackgrounds[positionIndex], alpha);
        SetSpriteAlpha(activeCoffeeIcons[positionIndex], alpha);
        SetSpriteAlpha(activeToastIcons[positionIndex], alpha);
        SetSpriteAlpha(activeBothIcons[positionIndex], alpha);
    }
    
    private void SetSpriteAlpha(GameObject obj, float alpha)
    {
        if (obj == null) return;
        
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color color = sr.color;
            sr.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}