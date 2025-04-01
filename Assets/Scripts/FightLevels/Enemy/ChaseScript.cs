using UnityEngine;

public class ChaseScript : MonoBehaviour
{
    public EnemyScript enemy;
    public float minDistance = 2f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //verifica se o jogador está dentro da area de ataque do inimigo
        float distance = Vector3.Distance(transform.position, collision.transform.position);

        if (collision.gameObject.CompareTag("Player"))
        {
            if (distance > minDistance)
            {
                enemy.ChaseMode = true;
                enemy.Attacking = false;
            }

            else
            {
                enemy.ChaseMode = false;
                enemy.Attacking = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //verifica se o jogador está na area de busca do inimigo
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.ChaseMode = false;
            enemy.Attacking = false;
        }
    }
}
