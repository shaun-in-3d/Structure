using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCollectionScript : MonoBehaviour
{
    [SerializeField] private DeliveryManager delivery;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(delivery.houses.Count);
        PlayerGame player = other.GetComponent<PlayerGame>();
        if (player)
        {
            delivery.completeDelivery();
        }
    }
}
