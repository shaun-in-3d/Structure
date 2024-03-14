using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDeliveryScript : MonoBehaviour
{
    [SerializeField] private DeliveryManager deliveryManager;

    private GameObject teaCollider;
    private void Awake()
    {
        deliveryManager.addHouse(this);
        teaCollider = gameObject.transform.GetChild(2).gameObject;

        disableHouse();
    }

    public void enableHouse()
    {
        teaCollider.active = true;
    }

    public void disableHouse()
    {
        teaCollider.active = false;
    }
}
