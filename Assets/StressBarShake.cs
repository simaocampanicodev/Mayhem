using UnityEngine;
using UnityEngine.UI;

public class StressBarShake : MonoBehaviour
{
    [SerializeField] private float intensity = 2.0f;
    [SerializeField] private float frequency = 30.0f;
    [SerializeField] private bool tremor = true;
    [SerializeField] private float durationTremor = 0.5f;

    private RectTransform rectTransform;
    private Vector3 position;
    private float time;
    private bool active = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        position = rectTransform.localPosition;

        if (tremor)
        {
            StartTremor();
        }
    }

    void Update()
    {
        if (!active) return;
        float offsetX = Mathf.PerlinNoise(Time.time * frequency, 0) * 2 - 1;
        float offsetY = Mathf.PerlinNoise(0, Time.time * frequency) * 2 - 1;
        Vector3 newPosition = position + new Vector3(offsetX, offsetY, 0) * intensity;
        rectTransform.localPosition = newPosition;
        
        if (!tremor)
        {
            time += Time.deltaTime;
            if (time >= durationTremor)
            {
                StopTremor();
            }
        }
    }
    public void StartTremor()
    {
        active = true;
        time = 0f;
    }
    public void StopTremor()
    {
        active = false;
        rectTransform.localPosition = position;
    }
}