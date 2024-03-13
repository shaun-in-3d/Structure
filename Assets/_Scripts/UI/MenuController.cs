using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    public Selectable firstSelectable;
    private List<Selectable> allSelectables = new List<Selectable>();
    private GameObject lastSelectedObject = null;

    void Awake()
    {
        GetComponentsInChildren(true, allSelectables);
        SetupUIListeners();
    }

    void Start()
    {
        Invoke(nameof(SelectFirstSelectable), 0.1f);
    }

    void Update()
    {
        HandleSelectionChange();
    }

    void SetupUIListeners()
    {
        foreach (Selectable selectable in allSelectables)
        {
            EventTrigger trigger = selectable.gameObject.GetComponent<EventTrigger>() ?? selectable.gameObject.AddComponent<EventTrigger>();

            var pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            pointerEnter.callback.AddListener((data) => { OnHover(selectable.gameObject); });
            trigger.triggers.Add(pointerEnter);

            var pointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            pointerExit.callback.AddListener((data) => { OnExit(selectable.gameObject); });
            trigger.triggers.Add(pointerExit);
        }
    }

    void SelectFirstSelectable()
    {
        if (firstSelectable != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
            lastSelectedObject = firstSelectable.gameObject;
            // Trigger animation for the first item as it becomes selected
            AnimateUIElement(firstSelectable.gameObject, true); // Add this line
        }
    }


    void HandleSelectionChange()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        if (currentSelected != lastSelectedObject)
        {
            // Animate last selected object back if it's not null
            if (lastSelectedObject != null)
            {
                AnimateUIElement(lastSelectedObject, false);
            }
            // Animate current selected object
            if (currentSelected != null)
            {
                AnimateUIElement(currentSelected, true);
            }
            lastSelectedObject = currentSelected;
        }
    }

    void OnHover(GameObject uiElement)
    {
        // Prevent re-animation if already selected
        if (uiElement != lastSelectedObject)
        {
            EventSystem.current.SetSelectedGameObject(uiElement);
        }
        
    }

    void OnExit(GameObject uiElement)
    {
        // Do not revert animation on exit if this is the currently selected object
        
        AnimateUIElement(uiElement, false);
    }

    void AnimateUIElement(GameObject uiElement, bool isSelected)
    {
        Transform labelTransform = uiElement.transform.Find("Label");
        if (labelTransform != null)
        {
            float targetX = isSelected ? 10f : 0f;
            labelTransform.DOLocalMoveX(targetX, 0.25f).SetEase(Ease.OutQuad);
        }

        
        if (AudioManagerSFX.Instance != null)
        {
            AudioManagerSFX.Instance.Play("Highlight");
        }

        
    }
}
