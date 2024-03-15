using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTemplateScript : MonoBehaviour
{
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private ShopScript shopScript; //For getting the type
    private void Awake()
    {
        deliveryManager.addTeaShop(gameObject, shopScript.type);
    }
}

