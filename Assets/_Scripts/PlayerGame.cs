using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerGame : MonoBehaviour
{
    [SerializeField] private int strikes = 3;
    [SerializeField] private DeliveryManager manager;

    private Delivery currentOrder;
    private teaTypes teaCarrying = teaTypes.none;
    private int bitesLeft=0;


    private void Awake()
    {
        currentOrder = manager.createDelivery(120,null);
        currentOrder.location.script.enableHouse();
    }

    public Delivery getCurrentDelivery()
    {
        return currentOrder;
    }
    

    public void eatTea()
    {
        if(teaCarrying==teaTypes.none)  //No tea to eat
        {
            return;
        }

        bitesLeft -= 1;
        if(bitesLeft==0)
        {
            teaCarrying= teaTypes.none; 
        }
    }


    public void completeOrder(Delivery newOrder)
    {
        //Increment score

        //Check for if player should be penalized (order wrong, eaten)

        if(currentOrder.type!=teaCarrying)  //Carrying the wrong tea
        {
            Debug.Log("Wrong order");
            //TODO: Add code for NPC telling player they're order is wrong

            strikes--;

            if(strikes==0)
            {
                //TODO: Add code for losing
            }

        }
        else
        {
            Debug.Log("Right order");
        }

        currentOrder.location.script.disableHouse();
        newOrder.location.script.enableHouse();
        currentOrder = newOrder;

        teaCarrying = teaTypes.none;
    }

    public void failOrder(Delivery newOrder)
    {
        strikes--;
        if(strikes==0)
        {
            //TODO: Add code for losing
        }

        currentOrder.location.script.disableHouse();
        newOrder.location.script.enableHouse();
        currentOrder = newOrder;
    }

    public bool takeTea(teaTypes type)
    {
        if(teaCarrying!=teaTypes.none)  //Already have tea
        {
            return false;
        }
        teaCarrying = type;
        return true;
    }
}
