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
<<<<<<< HEAD

    }
}
=======
        
    }
}
>>>>>>> aa6da6453032d24797c3654afb4f008cd66ccda5
