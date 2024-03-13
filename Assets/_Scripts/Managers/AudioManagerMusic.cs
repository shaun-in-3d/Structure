using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;


public class AudioManagerMusic : MonoBehaviour
{
    public static AudioManagerMusic Instance;

    [SerializeField]
    private AudioSource musicSource;
    public List<AudioClip> musicClips;
    private Dictionary<string, AudioClip> musicLibrary = new Dictionary<string, AudioClip>();

    [Range(0f, 1f)]
    public float globalVolume = 1.0f;
    
    
    public static event Action<float> OnVolumeChanged;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
       
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        musicSource.volume = globalVolume;

        foreach (AudioClip clip in musicClips)
        {
            if (!musicLibrary.ContainsKey(clip.name))
            {
                musicLibrary.Add(clip.name, clip);
            }
        }
    }

    public void PlayMusic(string musicName, float fadeTime = 0f)
    {
        if (musicLibrary.TryGetValue(musicName, out AudioClip clip))
        {
            if (fadeTime > 0f)
            {
                DOTween.To(() => musicSource.volume, x => musicSource.volume = x, 0f, fadeTime).OnComplete(() =>
                {
                    musicSource.Stop();
                    musicSource.clip = clip;
                    musicSource.Play();
                    DOTween.To(() => musicSource.volume, x => musicSource.volume = x, globalVolume, fadeTime);
                });
            }
            else
            {
                musicSource.Stop();
                musicSource.clip = clip;
                musicSource.Play();
                musicSource.volume = globalVolume;
            }
        }
        else
        {
            Debug.LogWarning("MusicManager: Requested music clip not found in library.");
        }
    }

    public void SetVolume(float volume)
    {
        globalVolume = Mathf.Clamp(volume, 0f, 1f);
        musicSource.volume = globalVolume;
        PlayerPrefs.SetFloat("MusicVolume", globalVolume);
        PlayerPrefs.Save();
        
        // Notify all listeners about the volume change
        OnVolumeChanged?.Invoke(globalVolume);
    }
}
