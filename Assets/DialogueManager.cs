using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

[System.Serializable] // This makes the class show up in the Unity Inspector.
public class CharacterSprite
{
    public string characterName;
    public Sprite sprite;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image characterImage;
    [SerializeField] private RectTransform dialoguePanel;
    [SerializeField] private CanvasGroup dialogueCanvasGroup;
    [SerializeField] private float dialogueMoveAmount = 30f;
    [SerializeField] private List<CharacterSprite> characterSpriteList;

    private Dictionary<string, Sprite> characterSprites;
    private bool isMessageDisplayed = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        characterSprites = new Dictionary<string, Sprite>();
        foreach (var item in characterSpriteList)
        {
            characterSprites[item.characterName] = item.sprite;
        }

        dialogueCanvasGroup.alpha = 0;
        dialogueCanvasGroup.blocksRaycasts = false;
    }

    public void ShowMessage(string characterName, string message)
    {
        if (isMessageDisplayed)
        {
            // Immediately start fading out if a message is currently displayed, then fade in the new one
            dialogueCanvasGroup.DOFade(0f, 0.25f).OnComplete(() =>
            {
                DisplayNewMessage(characterName, message);
            });
        }
        else
        {
            DisplayNewMessage(characterName, message);
        }
    }

    private void DisplayNewMessage(string characterName, string message)
    {
        isMessageDisplayed = true;

        dialogueText.text = message;
        if (characterSprites.ContainsKey(characterName))
        {
            characterImage.sprite = characterSprites[characterName];
        }
        else
        {
            Debug.LogWarning($"Character sprite for {characterName} not found.");
        }

        // Ensure dialogue panel and canvas group are properly reset before animating
        dialogueCanvasGroup.alpha = 0;
        dialogueCanvasGroup.blocksRaycasts = true;

        // Fade in the dialogue
        dialogueCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InOutQuad);

        // After a delay, start the hide sequence
        dialogueCanvasGroup.DOFade(0f, 0.5f).SetDelay(5f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            isMessageDisplayed = false;
            dialogueCanvasGroup.blocksRaycasts = false;
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ShowMessage("ExampleCharacter", "This is a test message!");
        }
    }
}