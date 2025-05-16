using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float[] range;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (i < range.Length)
                {
                    Vector3 spawnPosition = new Vector3(transform.position.x - range[i], -4, transform.position.z);
                    GameObject en = Instantiate(enemies[i], spawnPosition, Quaternion.identity);
                    EnemyScript enemyScript = en.GetComponent<EnemyScript>();
                    enemyScript.ChaseMode = true;
                }
            }
            Destroy(gameObject);
        }
    }
}
