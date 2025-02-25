using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private GameManager _gameManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            if (other.gameObject.name.StartsWith("Coffee"))
            {
                if (_gameManager == null) _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                _gameManager.RemoveSleep(20);
                Destroy(other.gameObject);
                return;
            }
        }
    }

}