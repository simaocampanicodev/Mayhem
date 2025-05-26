using UnityEngine;
using UnityEngine.UI;

public class StressBorders : MonoBehaviour
{
    [SerializeField] private Image topBorder;
    [SerializeField] private Image bottomBorder;
    [SerializeField] private Image leftBorder;
    [SerializeField] private Image rightBorder;

    [SerializeField] private float maxShakeIntensity = 10f;
    [SerializeField] private float maxAlpha = 0.6f;

    [SerializeField] private StressBarManager stressManager;

    private Vector3 topOrigPos;
    private Vector3 bottomOrigPos;
    private Vector3 leftOrigPos;
    private Vector3 rightOrigPos;

    void Start()
    {
        topOrigPos = topBorder.rectTransform.localPosition;
        bottomOrigPos = bottomBorder.rectTransform.localPosition;
        leftOrigPos = leftBorder.rectTransform.localPosition;
        rightOrigPos = rightBorder.rectTransform.localPosition;

        if (stressManager == null)
            stressManager = FindFirstObjectByType<StressBarManager>();
    }

    void Update()
    {
        if (stressManager == null) return;

        float stressPercent = stressManager.StressPercentage();

        float alpha = Mathf.Lerp(0f, maxAlpha, stressPercent);
        float shake = Mathf.Lerp(0f, maxShakeIntensity, stressPercent);

        SetBorderAlpha(topBorder, alpha);
        SetBorderAlpha(bottomBorder, alpha);
        SetBorderAlpha(leftBorder, alpha);
        SetBorderAlpha(rightBorder, alpha);

        ApplyShake(topBorder.rectTransform, topOrigPos, shake);
        ApplyShake(bottomBorder.rectTransform, bottomOrigPos, shake);
        ApplyShake(leftBorder.rectTransform, leftOrigPos, shake);
        ApplyShake(rightBorder.rectTransform, rightOrigPos, shake);
    }

    void SetBorderAlpha(Image img, float alpha)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    void ApplyShake(RectTransform rt, Vector3 originalPos, float intensity)
    {
        if (rt == null) return;
        Vector3 shakeOffset = new Vector3(
            Random.Range(-intensity, intensity),
            Random.Range(-intensity, intensity),
            0f);
        rt.localPosition = originalPos + shakeOffset;
    }
}
