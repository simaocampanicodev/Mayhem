using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class SilhouetteOrder : MonoBehaviour
{
    [SerializeField] private GameObject orderBubble;
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private SpriteRenderer orderIcon;
    [SerializeField] private Sprite toastSprite;
    [SerializeField] private Sprite coffeeSprite;
    [SerializeField] private Sprite bothSprite;
    [SerializeField] private float fadeDuration = 1.5f;

    private float duration;
    private Action onDestroy;
    [SerializeField] private OrderType currentOrder;
    private bool orderFulfilled = false;
    private bool isDestroying = false;
    
    private Collider2D myCollider;
    
    private StressBarManager stressManager;
    private OrderDisplay orderDisplay;
    private int seatIndex = -1;

    public enum OrderType
    {
        Toast,
        Coffee,
        Both
    }

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        stressManager = FindObjectOfType<StressBarManager>();
        orderDisplay = FindObjectOfType<OrderDisplay>();
    }

    public void Initialize(float duration, Action onDestroy)
    {
        this.duration = duration;
        this.onDestroy = onDestroy;
        
        GenerateRandomOrder();
        ShowOrder();
        StartCoroutine(FadeInOutSequence(duration));
        
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == transform)
            {
                seatIndex = i;
                break;
            }
        }
    }

    private void GenerateRandomOrder()
    {
        int randomOrder = UnityEngine.Random.Range(0, 3);
        currentOrder = (OrderType)randomOrder;
    }

    private void ShowOrder()
    {
        if (orderBubble != null)
            orderBubble.SetActive(true);
        
        if (orderText != null)
        {
            switch (currentOrder)
            {
                case OrderType.Toast:
                    orderText.text = "Tosta";
                    if (orderIcon != null && toastSprite != null)
                        orderIcon.sprite = toastSprite;
                    break;
                case OrderType.Coffee:
                    orderText.text = "Café";
                    if (orderIcon != null && coffeeSprite != null)
                        orderIcon.sprite = coffeeSprite;
                    break;
                case OrderType.Both:
                    orderText.text = "Tosta & Café";
                    if (orderIcon != null && bothSprite != null)
                        orderIcon.sprite = bothSprite;
                    break;
            }
        }
    }

    private void Update()
    {
        if (!isDestroying && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckPlayerDelivery();
        }
    }

    private void CheckPlayerDelivery()
    {
        if (orderFulfilled)
            return;
        
        Collider2D[] colliders = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0);
        bool playerInside = false;
        
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                playerInside = true;
                break;
            }
        }
        
        if (playerInside)
        {
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            bool isCorrect = false;
            
            if (inventory != null)
            {
                isCorrect = inventory.CheckAndDeliverOrder(currentOrder);
            }
            
            if (stressManager != null)
            {
                if (isCorrect)
                {
                    stressManager.DecreaseStress(10f);
                    ShowFeedback(true);
                }
                else
                {
                    stressManager.IncreaseStress(20f);
                    ShowFeedback(false);
                }
            }
            
            orderFulfilled = true;
            
            if (orderDisplay != null && seatIndex >= 0)
            {
                orderDisplay.HideOrder(seatIndex);
            }
            
            StartDestroySequence();
        }
    }

    private void ShowFeedback(bool isCorrect)
    {
        if (orderText != null)
        {
            
        }
    }

    private void StartDestroySequence()
    {
        if (isDestroying) return;
        
        isDestroying = true;
        StopAllCoroutines();
        StartCoroutine(FeedbackAndFadeOut());
    }
    
    private IEnumerator FeedbackAndFadeOut()
    {
        yield return new WaitForSeconds(1.5f);
        yield return Fade(1, 0, fadeDuration);
        
        if (onDestroy != null)
            onDestroy();
            
        Destroy(gameObject);
    }

    private IEnumerator FadeInOutSequence(float visibleTime)
    {
        yield return Fade(0, 1, fadeDuration);
        
        float timeRemaining = visibleTime - fadeDuration;
        
        while (timeRemaining > 0 && !isDestroying)
        {
            yield return null;
            timeRemaining -= Time.deltaTime;
        }
        
        if (!isDestroying)
        {
            isDestroying = true;
            
            yield return Fade(1, 0, fadeDuration);
            
            if (onDestroy != null)
                onDestroy();
                
            Destroy(gameObject);
        }
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        float elapsed = 0;
        Color color = sr.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            sr.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(color.r, color.g, color.b, to);
    }

    public bool IsOrderFulfilled()
    {
        return orderFulfilled;
    }
    
    public OrderType GetCurrentOrder()
    {
        return currentOrder;
    }
}