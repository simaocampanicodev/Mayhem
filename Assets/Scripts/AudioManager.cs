using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource collectSound;
    public AudioSource deliverSound;
    public AudioSource wrongSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayToastSound()
    {
        collectSound.Play();
    }

    public void PlayDeliverSound()
    {
        deliverSound.Play();
    }
    public void PlayWrongDeliverSound()
    {
        wrongSound.Play();
    }
}
