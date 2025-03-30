using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Vector2 move_input;
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

    void Awake()
    {
        //lê os inputs
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        //verifica se o jogador clicou nas setas/joystick ou espaço/cruz
        inputActions.Player.Enable();
        inputActions.Player.Move.Enable();
        inputActions.Player.Interact.Enable();
        inputActions.Player.Enter.Enable();
    }

    private void OnDisable()
    {
        //idem 
        inputActions.Player.Disable();
        inputActions.Player.Move.Disable();
        inputActions.Player.Interact.Disable();
        inputActions.Player.Enter.Disable();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        if (moneyUI != null)
        {
            moneyUI.UpdateMoney(money);
        }
        // Inicializar o NPCManager
        npcManager = FindFirstObjectByType<NPCManager>();
        if (npcManager == null)
        {
            Debug.LogError("NPCManager não encontrado na cena!");
        }
    }

    void Update()
    {
        move_input = inputActions.Player.Move.ReadValue<Vector2>();
        float value = move_input.x;
        if (value < 0) value *= -1;

        // Verificar se o objeto tem um Animator antes de tentar usá-lo
        if (animator != null)
        {
            animator.SetFloat("speed", value);
        }

        if (insideNPC == true && npcOrder != null)
        {
            if (inputActions.Player.Interact.WasPressedThisFrame())
            {
                Debug.Log("Pedido do NPC: " + npcOrder.currentOrder);
            }
            if (inputActions.Player.Enter.WasPressedThisFrame())
            {
                if (npcOrder != null)
                {
                    CheckOrder(npcOrder);
                }
            }
        }
        if (insideToaster == true && inputActions.Player.Enter.WasPressedThisFrame())
        {
            if (!hasToast)
            {
                hasToast = true;
                AudioManager.instance.PlayToastSound();
                Debug.Log("You got a toast!");
            }
        }
        if (insideCoffeeMachine == true && inputActions.Player.Enter.WasPressedThisFrame())
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
        if (other.CompareTag("CoffeeMachine"))
        {
            insideCoffeeMachine = false;
        }
        if (other.CompareTag("NPC"))
        {
            insideNPC = false;
            npcOrder = null;
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
