using UnityEngine;

public class PopUpDamage : MonoBehaviour
{
    [SerializeField] private Vector2 InitialVelocity;
    [SerializeField] private float lifetime = .5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = InitialVelocity;
        Destroy(gameObject, lifetime);
        
    }
}
