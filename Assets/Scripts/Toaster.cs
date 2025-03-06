using UnityEngine;

public class Toaster : MonoBehaviour
{
    public bool hasToast = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && PlayerNearby())
        {
            AudioManager.instance.PlayCollectSound();
            hasToast = true;
            Debug.Log("Pegou uma Torrada!");
        }
    }

    private bool PlayerNearby()
    {
        return Vector2.Distance(transform.position, FindObjectOfType<PlayerManager>().transform.position) < 2f;
    }
}
