using UnityEngine;
using System.Collections;

public class TimerLevel : MonoBehaviour
{
    [SerializeField] private float time;
    private bool Done = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0.0f && !Done)
        {
            timerEnded();
        }
    }

    void timerEnded()
    {
        Done = true;
        GetComponent<SceneFadeOut>().enabled = true;
        KeepGameData data = GameObject.Find("KeepFightData").GetComponent<KeepGameData>();
        data.KeepFightDataAfterLoad();
    }
}
