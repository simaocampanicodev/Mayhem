using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneHospital : MonoBehaviour
{
    [SerializeField] private GameObject good1;
    [SerializeField] private GameObject good2;
    [SerializeField] private GameObject bad1;
    [SerializeField] private GameObject bad2;
    private float stress = 50;
    private float stressFactor;
    void Start()
    {
        KeepGameData data = GameObject.Find("KeepFightData").GetComponent<KeepGameData>();
        GameObject dataObj = GameObject.Find("KeepCoffeeData");
        if (dataObj != null)
        {
            KeepGameData datatwo = dataObj.GetComponent<KeepGameData>();
            stress = datatwo.stress;
        }
        stressFactor = 1f - (stress / 100f);
        int life = data.life;
        if (life > 75*stressFactor) { Instantiate(good1); }
        if (life <= 75*stressFactor && life > 50*stressFactor) { Instantiate(good2); }
        if (life <= 50*stressFactor && life > 25*stressFactor) { Instantiate(bad1); }
        if (life <= 25*stressFactor) { Instantiate(bad2); }
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(6f);
        
        SceneManager.LoadScene("Hospital");
    }
}