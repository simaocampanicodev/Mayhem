using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float value = Input.GetAxis("Horizontal");
        if (value < 0) value *= -1;
        animator.SetFloat("speed", value);

    }
}
