using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCollectionScript : MonoBehaviour
{
    [SerializeField] private DeliveryManager delivery;
    private void OnTriggerEnter(Collider other)
    {
        PlayerGame player = other.GetComponent<PlayerGame>();
        if (player)
        {
            delivery.completeDelivery();
        }
    }
}
