using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class RageScript : MonoBehaviour
{
    [SerializeField]
    private int _rage;
    public int rage
    {
        get { return _rage; }
        set
        {
            _rage = Mathf.Clamp(value, 0, MAXRAGE);
            UpdateBar();
        }
    }
    private const int MAXRAGE = 100;
    [SerializeField] private Image ragebar;
    private InputSystem_Actions inputActions;
    private PlayerScript plr;
    private bool draining = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //lê os inputs
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Jump.Enable();
        //verifica se o jogador clicou nas setas/joystick ou espaço/cruz
        inputActions.Player.Jump.performed += Ragebait;

    }
    private void OnDisable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Jump.Disable();
        //verifica se o jogador clicou nas setas/joystick ou espaço/cruz
        inputActions.Player.Jump.performed -= Ragebait;

    }
    void Start()
    {
        UpdateBar();
        plr = FindAnyObjectByType<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateBar()
    {
        float ragePercent = Mathf.Clamp01((float)rage / MAXRAGE);
        ragebar.fillAmount = ragePercent;
    }

    private void Ragebait(InputAction.CallbackContext context)
    {
        if (!draining && rage >= 90) { StartCoroutine(DrainBar()); }
    }

    IEnumerator DrainBar()
    {
        draining = true;
        plr.multiplier = 2;
        while (rage > 0)
        {
            rage -= 1;
            float ragePercent = Mathf.Clamp01((float)rage / MAXRAGE);
            ragebar.fillAmount = ragePercent;
            yield return new WaitForSeconds(0.1f);
        }
        plr.multiplier = 1;
        draining = false;
    }
}
