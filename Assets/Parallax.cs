using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float speed;
    private float lastCamPosition;

    void Start()
    {
        lastCamPosition = cam.transform.position.x;
    }
    private void LateUpdate()
    {
        float deltaX = cam.transform.position.x - lastCamPosition;

        if (deltaX != 0)
        {
            transform.position += new Vector3(deltaX / speed, 0, 0);
        }

        lastCamPosition = cam.transform.position.x;
    }
}
