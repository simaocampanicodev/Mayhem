using UnityEngine;

public class GruntScript : MonoBehaviour
{
    [SerializeField] private AudioClip[] grunts;
    [SerializeField] private AudioClip[] deaths;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip GruntSound {
        get 
        {
            int sound = Random.Range(0, grunts.Length);
            return grunts[sound];
        }
    }

    public AudioClip DeathSound {
        get 
        {
            int sound = Random.Range(0, deaths.Length);
            return deaths[sound];
        }
    }
}
