using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GruntScript hurtSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource radioSource = gameObject.AddComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * 15, ForceMode2D.Impulse);
        AudioClip grunt = hurtSound.DeathSound;
        radioSource.PlayOneShot(grunt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
