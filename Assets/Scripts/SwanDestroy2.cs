using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwanDestroy2 : MonoBehaviour
{
    public Collider playerCollider; // Reference to the player's collider
    public AudioSource destroySoundSource; // Reference to the AudioSource component
    public AudioSource punch; // Reference to the AudioSource component
    public AudioSource crunch; // Reference to the AudioSource component
    public AudioSource audioToStop; // Reference to the specific AudioSource you want to stop
    public Image screenFadeImage; // Reference to the Image component for screen fade

    private bool isFading = false;

    void Start()
    {
        // Assuming you have assigned the Image component in the Unity Editor
        // screenFadeImage = GetComponent<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the player
        if (other == playerCollider && !isFading)
        {
            PlayDestroySequence();
        }
    }

    void PlayDestroySequence()
    {
        isFading = true;

        // Stop the specific audio source
        if (audioToStop != null)
        {
            audioToStop.Stop();
        }

        // Play the destroy sound
        destroySoundSource.Play();
        punch.Play();
        crunch.Play();

        // Enable the screen fade image
        screenFadeImage.enabled = true;

        // Wait for 10 seconds before loading the next scene
        StartCoroutine(WaitAndLoadNextScene());
    }

    IEnumerator WaitAndLoadNextScene()
    {
        yield return new WaitForSeconds(7f);

        // Load the next scene (change "NextSceneName" to your actual scene name)
        SceneManager.LoadScene("EndCreditsBad");
    }
}

