using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalLevel : MonoBehaviour
{
    [SerializeField] private Image black;
    [SerializeField] private float fadeDuration;
    [SerializeField] private string num = "Video";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player") {
            SceneManager.LoadScene(num);
        }
    }
}
