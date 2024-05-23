using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> sounds = new List<AudioClip>(); // List of sounds
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // Play sound based on index
    public void PlaySound(int index)
    {
        if (index >= 0 && index < sounds.Count) // Check if index is within the bounds of the list
        {
            audioSource.PlayOneShot(sounds[index]); // Play the sound at the specified index
        }
        else
        {
            Debug.LogWarning("Sound index out of range!");
        }
    }
}

