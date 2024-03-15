using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerGame : MonoBehaviour
{
    [SerializeField] private int strikes = 3;
    [SerializeField] private DeliveryManager manager;
    [SerializeField] private DirectionArrow arrow;

    private Delivery currentOrder;
    private teaTypes teaCarrying = teaTypes.none;
    private int bitesLeft=0;


    private void Awake()
    {
        currentOrder = manager.createDelivery(5,null);
        currentOrder.location.script.enableHouse();
    }

    public Delivery getCurrentDelivery()
    {
        return currentOrder;
    }

    public bool tickTimer(float deltaTime)  //Reduce the timer on the current order and return whether they have time left
    {
        currentOrder.time -= deltaTime;
        return currentOrder.time > 0;
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

        updateOrder(newOrder);

        teaCarrying = teaTypes.none;
    }

    public void failOrder(Delivery newOrder)
    {
        strikes--;
        if(strikes==0)
        {
            //TODO: Add code for losing
        }

        Debug.Log("Delivery failed to make");
        updateOrder(newOrder);
    }

    private void updateOrder(Delivery newOrder)
    {
        if(currentOrder.location.script!=null)
        {
            currentOrder.location.script.disableHouse();
        }
        
        newOrder.location.script.enableHouse();
        currentOrder = newOrder;
        arrow.targetObject = newOrder.location.script.gameObject.transform;
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
