using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadingScript : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField] private string sceneName = "";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneName);
    }

}
