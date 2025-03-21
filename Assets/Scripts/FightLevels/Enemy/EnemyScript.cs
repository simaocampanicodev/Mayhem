using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisão detectada com: " + other.gameObject.name);
        if (other.gameObject.tag == "Punch") {
            sr.color = new Color(1,0,0);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Punch") {
            sr.color = new Color(1,1,1);
        }
    }
}
