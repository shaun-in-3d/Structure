using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    [SerializeField] public teaTypes type;
    [SerializeField] private DeliveryManager deliveryManager;
    private void OnTriggerStay(Collider other)
    {
        deliveryManager.giveTea(type);
    }
}
