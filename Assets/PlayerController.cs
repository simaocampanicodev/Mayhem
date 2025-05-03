using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;   

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("CoffeeShop 2.0");
        }
    }
}
