using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private bool hasCoffee = false;
    private bool hasToast = false;
    private int money = 0;

    private bool correctOrder = false;
    private Animator animator;
    public AudioSource AudioPlayer;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float value = Input.GetAxis("Horizontal");
        if (value < 0) value *= -1;
        animator.SetFloat("speed", value);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Verifica se está colidindo com um NPC para entregar pedido
            Collider2D npcCollider = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("NPC"));
            if (npcCollider != null)
            {
                NPCOrder npcOrder = npcCollider.GetComponent<NPCOrder>();
                if (npcOrder != null)
                {
                    CheckOrder(npcOrder);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            AudioPlayer.Play();
            if (other.gameObject.name.StartsWith("Coffee"))
            {
                if (_gameManager == null) _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                _gameManager.RemoveSleep(20);
                Destroy(other.gameObject);
                return;
            }
            if (other.gameObject.name.StartsWith("EnergyDrink"))
            {
                if (_gameManager == null) _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                _gameManager.RemoveSleep(30);
                Destroy(other.gameObject);
                return;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (other.CompareTag("Toaster"))
            {
                hasToast = true;
                Debug.Log("Pegou uma tosta!");
            }
            else if (other.CompareTag("CoffeeMachine"))
            {
                hasCoffee = true;
                Debug.Log("Pegou um café!");
            }
            else if (other.CompareTag("NPC"))
            {
                NPCOrder npcOrder = other.GetComponent<NPCOrder>();
                if (npcOrder != null)
                {
                    CheckOrder(npcOrder);
                }
            }
        }
    }

    public void CheckOrder(NPCOrder npcOrder)
    {

        Debug.Log("Pedido do NPC: " + npcOrder.currentOrder);

        if (npcOrder.currentOrder == NPCOrder.OrderType.Coffee && hasCoffee)
        {
            correctOrder = true;
        }
        else if (npcOrder.currentOrder == NPCOrder.OrderType.Toast && hasToast)
        {
            correctOrder = true;
        }
        else if (npcOrder.currentOrder == NPCOrder.OrderType.Both && hasCoffee && hasToast)
        {
            correctOrder = true;
        }

        if (correctOrder)
        {
            AudioManager.instance.PlayDeliverSound();
            money += 10;
            Debug.Log("Pedido correto! Dinheiro: " + money);
        }
        else
        {
            money -= 5;
            Debug.Log("Pedido errado! Multa no salário. Dinheiro: " + money);
        }

        hasCoffee = false;
        hasToast = false;
        FindObjectOfType<NPCManager>().RemoveCustomer();
    }
}
