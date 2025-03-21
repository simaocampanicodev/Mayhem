using UnityEngine;

public class PopUpDamage : MonoBehaviour
{
    public Vector2 InitialVelocity;
    public Rigidbody2D rb;
    public float lifetime = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = InitialVelocity;
        Destroy(gameObject, lifetime);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
