using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private NPCOrderUI npcOrderUI;
    [SerializeField] private MoneyUI moneyUI;
    private bool hasCoffee = false;
    private bool hasToast = false;
    private int money = 0;
    private bool insideNPC = false;

    private bool insideToaster = false;

    private bool insideCoffeeMachine = false;

    private bool correctOrder = false;
    private Animator animator;
    public AudioSource AudioPlayer;
    private NPCOrder npcOrder;
    private NPCManager npcManager;


    void Start()
    {
        animator = GetComponent<Animator>();
        if (moneyUI != null)
        {
            moneyUI.UpdateMoney(money);
        }
    }

    void Update()
    {
        float value = Input.GetAxis("Horizontal");
        if (value < 0) value *= -1;
        animator.SetFloat("speed", value);

        if (insideNPC == true && npcOrder != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Pedido do NPC: " + npcOrder.currentOrder);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (npcOrder != null)
                {
                    CheckOrder(npcOrder);
                }
            }
        }
        if (insideToaster == true && Input.GetKeyDown(KeyCode.Return))
        {
            if (!hasToast)
            {
                hasToast = true;
                AudioManager.instance.PlayToastSound();
                Debug.Log("You got a toast!");
            }
        }
        if (insideCoffeeMachine == true && Input.GetKeyDown(KeyCode.Return))
        {
            if (!hasCoffee)
            {
                hasCoffee = true;
                AudioManager.instance.PlayToastSound();
                Debug.Log("You got a coffee!");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Toaster"))
        {
            insideToaster = false;
        }
        if (other.CompareTag("Coffee Machine"))
        {
            insideCoffeeMachine = false;
        }
        if (other.CompareTag("NPC"))
        {
            insideNPC = false;
            npcOrder = null;

            if (npcOrderUI != null)
            {
                npcOrderUI.HideOrder();
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Toaster"))
        {
            insideToaster = true;
        }
        if (other.CompareTag("CoffeeMachine"))
        {
            insideCoffeeMachine = true;
        }
        if (other.CompareTag("NPC"))
        {
            insideNPC = true;
            npcOrder = other.GetComponent<NPCOrder>();

            if (npcOrder != null && npcOrderUI != null)
            {
                npcOrderUI.UpdateOrder(npcOrder.currentOrder);
            }
        }
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
    public void CheckOrder(NPCOrder npcOrder)
    {

        Debug.Log("Pedido do NPC: " + npcOrder.currentOrder);

        correctOrder = false;

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
            money += 15;
            Debug.Log("Pedido correto! Dinheiro: " + money);
            _gameManager.RemoveSleep(15);
        }
        else
        {
            AudioManager.instance.PlayWrongDeliverSound();
            money -= 5;
            Debug.Log("Pedido errado! Multa no salário. Dinheiro: " + money);
            _gameManager.RemoveSleep(-10);
        }
        hasCoffee = false;
        hasToast = false;
        npcManager.RemoveCustomer();
        if (moneyUI != null)
        {
            moneyUI.UpdateMoney(money);
        }
    }
}
