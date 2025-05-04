using UnityEngine;

public class ShowStuff : MonoBehaviour
{
    [SerializeField] private GameObject[] objs;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player") {
            foreach (GameObject obj in objs) {
                obj.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player") {
            foreach (GameObject obj in objs) {
                obj.SetActive(false);
            }
        }
    }
}
