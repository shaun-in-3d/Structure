using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(.5f, 1.5f)]
    public float pitch = 1.0f;
    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManagerSFX : MonoBehaviour
{
    public static AudioManagerSFX Instance;
    public static event Action<float> OnVolumeChanged;

    [Range(0f, 1f)]
    public float globalVolume = 1.0f; // Global volume control

    public Sound[] sounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * globalVolume; // Apply global volume at start
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        Debug.Log($"Playing sound: {name}");
        s.source.volume = s.volume * globalVolume;
        s.source.Play();
        
        Debug.Log($"Global Volume: {globalVolume}, Sound Volume: {s.volume}");

    }


    public void Stop(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    // Method to update global volume

    
    public void SetVolume(float volume)
    {
        globalVolume = Mathf.Clamp(volume, 0f, 1f); // Ensure the volume is within the valid range.
        foreach (Sound sound in sounds)
        {
            if (sound.source != null) // Check if the AudioSource is set
            {
                sound.source.volume = sound.volume * globalVolume; // Adjust each sound's volume based on the global volume.
            }
        }
        
        OnVolumeChanged?.Invoke(globalVolume); // Trigger subscribers about volume change.

        // Save the volume setting to PlayerPrefs to persist between game sessions.
        PlayerPrefs.SetFloat("SFXVolume", globalVolume); // Use a different key for SFX volume if you also have music volume.
        PlayerPrefs.Save();
    }
    

}