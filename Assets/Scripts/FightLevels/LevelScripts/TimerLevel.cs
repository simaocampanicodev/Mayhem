using UnityEngine;
using System.Collections;

public class TimerLevel : MonoBehaviour
{
    [SerializeField] private float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0.0f)
        {
            timerEnded();
        }
    }

    void timerEnded()
    {
        gameObject.GetComponent<SceneFadeOut>().enabled = true;
    }
}
