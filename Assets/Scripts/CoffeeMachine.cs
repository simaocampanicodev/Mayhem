using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public bool hasCoffee = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && PlayerNearby())
        {
            AudioManager.instance.PlayCollectSound();
            hasCoffee = true;
            Debug.Log("Pegou um Café!");
        }
    }

    private bool PlayerNearby()
    {
        return Vector2.Distance(transform.position, FindObjectOfType<PlayerManager>().transform.position) < 2f;
    }
}
