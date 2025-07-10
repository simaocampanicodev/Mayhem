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
    [SerializeField] private string sceneName;
    [SerializeField] private string sceneSkipName;
    private int life;
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
        if (life > 75*stressFactor) { Instantiate(good1); }
        if (life <= 75*stressFactor && life > 50*stressFactor) { Instantiate(good2); }
        if (life <= 50*stressFactor && life > 25*stressFactor) { Instantiate(bad1); }
        if (life <= 25*stressFactor) { Instantiate(bad2); }
        StartCoroutine(LoadSceneAfterDelay(life));
    }

    private IEnumerator LoadSceneAfterDelay(int life)
    {
        yield return new WaitForSeconds(6f);
        if ((float)life >= 90 * stressFactor) { SceneManager.LoadScene(sceneSkipName); } // if the life is full go to this scene
        else { SceneManager.LoadScene(sceneName); }
    }
}