using UnityEngine;

public class KeepGameData : MonoBehaviour
{
    public int life { get; set; }
    public float stress { get; set; }
    public int money { get; set; }
    public void KeepFightDataAfterLoad()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        PlayerScript player = playerObj.GetComponent<PlayerScript>();
        life = player.life;
        DontDestroyOnLoad(gameObject);
    }
    public void KeepCoffeeDataAfterLoad()
    {
        StressBarManager stressObj = FindFirstObjectByType<StressBarManager>();
        StressBarManager stressScript = stressObj.GetComponent<StressBarManager>();
        MoneyManager moneyObj = FindFirstObjectByType<MoneyManager>();
        MoneyManager moneyScript = moneyObj.GetComponent<MoneyManager>();
        stress = stressScript.actualStress;
        money = moneyScript.currentMoney;
        DontDestroyOnLoad(gameObject);
    }
}