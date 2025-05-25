using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSkipper : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private bool useSceneIndex = false;
    [SerializeField] private int nextSceneIndex = 0;
    [SerializeField] private KeyCode skipKey = KeyCode.K;

    void Update()
    {
        if (Input.GetKeyDown(skipKey))
        {
            SkipToNextScene();
        }
    }

    void SkipToNextScene()
    {
        if (useSceneIndex)
        {
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadSceneAsync(nextSceneIndex);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadSceneAsync(nextSceneName);
            }
        }
    }

    public void SkipScene()
    {
        SkipToNextScene();
    }

    public void SetNextScene(string sceneName)
    {
        nextSceneName = sceneName;
        useSceneIndex = false;
    }

    public void SetNextScene(int sceneIndex)
    {
        nextSceneIndex = sceneIndex;
        useSceneIndex = true;
    }
}