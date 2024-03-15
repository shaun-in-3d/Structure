using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        image.fillAmount = 1;
    }
    public void setStamina(float fillAmount)
    {
        image.fillAmount = fillAmount;
    }
}
