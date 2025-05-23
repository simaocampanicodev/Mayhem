using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneHospital : MonoBehaviour
{
    [SerializeField] private GameObject good1;
    [SerializeField] private GameObject good2;
    [SerializeField] private GameObject bad1;
    [SerializeField] private GameObject bad2;
    void Start()
    {
        KeepGameData data = GameObject.Find("KeepFightData").GetComponent<KeepGameData>();
        int life = data.life;
        if (life > 75) { Instantiate(good1); }
        if (life <= 75 && life > 50) { Instantiate(good2); }
        if (life <= 50 && life > 25) { Instantiate(bad1); }
        if (life <= 25) { Instantiate(bad2); }
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(6f);
        
        SceneManager.LoadScene("Hospital");
    }
}