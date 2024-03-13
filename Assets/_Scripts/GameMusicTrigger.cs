using UnityEngine;

public class GameMusicTrigger : MonoBehaviour
{
    [SerializeField]
    private string musicName = "lofi-relax"; // Track name (from lib), music must also be added to the Music Manager
    

    [SerializeField]
    private float fadeDuration = 1.0f; // Duration of the fade in seconds

  
    void Start()
    {
        if (AudioManagerMusic.Instance != null)
        {
            AudioManagerMusic.Instance.PlayMusic(musicName, fadeDuration);
        }
    }
}