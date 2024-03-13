using UnityEngine;

public class SystemsSingleton : MonoBehaviour
{
    public static SystemsSingleton Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy this instance because it's a duplicate
        }
    }
}