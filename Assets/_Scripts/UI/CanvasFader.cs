using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems; 
using UnityEngine.UI;
public class CanvasFader : MonoBehaviour  //REQUIRES A CANVAS GROUP COMPONENT ON THE CANVAS
{
    private CanvasGroup canvasGroup;
    [SerializeField]
    private float fadeDuration = 0.5f;
    [SerializeField]
    private bool startVisible = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroupFader: No CanvasGroup found on the GameObject.", this);
        }
    }

    private void Start()
    {
        InitializeVisibility();
    }

    private void InitializeVisibility()
    {
        if (startVisible)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // Function to fade in the CanvasGroup
    public void FadeIn()
    {
        if (canvasGroup == null) return;
        
        canvasGroup.blocksRaycasts = true; // Enable interaction with the UI element
        canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);
        SelectFirstButton();
    }

    // Function to fade out the CanvasGroup
    public void FadeOut()
    {
        if (canvasGroup == null) return;
        
        canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true)
        .OnComplete(() => canvasGroup.blocksRaycasts = false); // Disable interaction after fade out
        
    }

    // Optional: Toggle the visibility of the CanvasGroup
    public void ToggleVisibility()
    {
        if (canvasGroup == null) return;
        
            if (canvasGroup.alpha == 0f)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
    }
    
    void OnDestroy()
    {
        // This kills all tweens associated with this component
        this.DOKill();
    }
    
    private void SelectFirstButton()
    {
        Button firstButton = GetComponentInChildren<Button>(true); // true to include inactive children
        if (firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        }
    }

}