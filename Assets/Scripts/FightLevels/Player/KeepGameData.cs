using UnityEngine;

public class KeepGameData : MonoBehaviour
{
    public int life { get; set; }
    public void KeepFightDataAfterLoad()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        PlayerScript player = playerObj.GetComponent<PlayerScript>();
        life = player.life;
        DontDestroyOnLoad(gameObject);
    }
}