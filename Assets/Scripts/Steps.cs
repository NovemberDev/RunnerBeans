using UnityEngine;

public class Steps : MonoBehaviour {

    public AudioSource audioStepSound;

    private void Start()
    {
        audioStepSound = GetComponent<AudioSource>();
    }

    public void PlayStepSound()
    {
        audioStepSound.Play();
    }
}
