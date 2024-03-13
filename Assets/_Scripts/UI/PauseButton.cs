using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Make sure to include this for InputActionReference

public class PauseButton : MonoBehaviour
{
    [SerializeField]
    private InputActionReference pauseActionReference; // Assign this in the Unity Inspector

    private GameObject pauseMenuGO;
    private CanvasFader canvasFader;

    private Button pauseButton; // Add this declaration

    private void Awake()
    {

        if (pauseActionReference == null) return;
        
        pauseActionReference.action.Enable();         // Ensure the Input Action is enabled

        pauseButton = GetComponent<Button>();         // Get the Button component attached to this GameObject

    }
    
    private void Start()
    {
        pauseMenuGO = GameObject.Find("PauseMenu");
        if (pauseMenuGO == null)
        {  
            Debug.LogError("PauseMenu GameObject not found.");  
            return; 
        }
        
        canvasFader = pauseMenuGO.GetComponent<CanvasFader>();
        if (canvasFader == null)
        {  
            Debug.LogError("CanvasFader not found.");  
            return; 
        }
        
        // Make sure pauseButton is not null before adding listener
        if (pauseButton != null)
        {
            // Assign the ToggleVisibility method to the button's onClick event
            pauseButton.onClick.AddListener(canvasFader.ToggleVisibility);
        }
    }

    private void OnEnable()
    {
        if (pauseActionReference == null) return;
        
        pauseActionReference.action.performed += _ => TogglePauseMenu();
        
    }

    private void OnDisable()
    {
        if (pauseActionReference == null) return;
        
        pauseActionReference.action.performed -= _ => TogglePauseMenu();
        
    }

    private void TogglePauseMenu()
    {
        canvasFader?.ToggleVisibility();
    }
}
