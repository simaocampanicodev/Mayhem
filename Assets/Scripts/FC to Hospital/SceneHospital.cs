using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneHospital : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(6f);
        
        SceneManager.LoadScene("Hospital");
    }
}