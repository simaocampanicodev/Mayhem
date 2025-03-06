using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private GameManager _gameManager;
    private CoffeeMachine coffeeMachine;
    private Toaster toaster;
    private bool hasCoffee = false;
    private bool hasToast = false;
    private int money = 0;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        coffeeMachine = FindObjectOfType<CoffeeMachine>();
        toaster = FindObjectOfType<Toaster>();
    }
    void Update()
    {
        float value = Input.GetAxis("Horizontal");
        if (value < 0) value *= -1;
        animator.SetFloat("speed", value);
        if (Input.GetKeyDown(KeyCode.Return) && IsNearNPC())

        {
            NPCOrder npcOrder = FindObjectOfType<NPCOrder>();
            if (npcOrder != null)
            {
                CheckOrder(npcOrder);
            }
        }
    }

    public AudioSource AudioPlayer;
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
    public bool IsNearNPC()
    {
        return Vector2.Distance(transform.position, FindObjectOfType<NPCManager>().transform.position) < 2f;
    }

    public void CheckOrder(NPCOrder npcOrder)
    {
        bool correctOrder = false;

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