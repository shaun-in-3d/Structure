using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SliderType { Music, SFX }

public class VolumeSlider : MonoBehaviour
{
    public SliderType sliderType;
    private Slider slider;
    

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        slider.onValueChanged.AddListener(HandleVolumeChange);
        if (AudioManagerMusic.Instance == null) return;
            
        // Subscribe to the appropriate event based on the slider type
        if (sliderType == SliderType.Music)
        {
            AudioManagerMusic.OnVolumeChanged += UpdateSliderValue;
            slider.value = AudioManagerMusic.Instance.globalVolume; // Initialize the slider's value
        }
        else if (sliderType == SliderType.SFX)
        {
            AudioManagerSFX.OnVolumeChanged += UpdateSliderValue;
            slider.value = AudioManagerSFX.Instance.globalVolume; // Initialize the slider's value
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        slider.onValueChanged.RemoveAllListeners();
        
        // Unsubscribe from the event
        if (sliderType == SliderType.Music)
        {
            AudioManagerMusic.OnVolumeChanged -= UpdateSliderValue;
        }
        else if (sliderType == SliderType.SFX)
        {
            AudioManagerSFX.OnVolumeChanged -= UpdateSliderValue;
        }
    }

    private void HandleVolumeChange(float volume)
    {
        if (sliderType == SliderType.Music)
        {
            AudioManagerMusic.Instance.SetVolume(volume);
        }
        else if (sliderType == SliderType.SFX)
        {
            AudioManagerSFX.Instance.SetVolume(volume);
        }
    }

    public void UpdateSliderValue(float volume)
    {
        slider.SetValueWithoutNotify(volume);
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateVolumeSettings(); // Call the same method to update the slider's value
    }
    
    public void UpdateVolumeSettings()
    {
        if (sliderType == SliderType.Music && AudioManagerMusic.Instance != null)
        {
            UpdateSliderValue(AudioManagerMusic.Instance.globalVolume);
        }
        else if (sliderType == SliderType.SFX && AudioManagerSFX.Instance != null)
        {
            UpdateSliderValue(AudioManagerSFX.Instance.globalVolume);
        }
    }

}