using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ImageSet
{
    public ImageName name;
    public Sprite[] sprites;
}

public enum ImageName
{
    none,
    boba,
    green,
    english,
    iced,
    microwave
}

public class TeaImageManager : MonoBehaviour
{
    public static TeaImageManager Instance { get; private set; }

    // This list will be visible in the editor, allowing you to assign sprites to each category
    public List<ImageSet> imageSets;

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
    }

    public Sprite GetSprite(ImageName name, int number)
    {
        // Find the ImageSet for the given name
        ImageSet set = imageSets.Find(s => s.name == name);
        if (set != null && number >= 0 && number < set.sprites.Length)
        {
            return set.sprites[number];
        }
        else
        {
            Debug.LogWarning("Sprite not found for given name and number");
            return null;
        }
    }

    // Example usage method to display an image
    public void DisplayImage(Image targetImage, ImageName name, int number)
    {
        Sprite sprite = GetSprite(name, number);
        if (sprite != null)
        {
            targetImage.sprite = sprite;
        }
    }
}