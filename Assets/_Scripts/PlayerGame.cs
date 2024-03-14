using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerGame : MonoBehaviour
{
    [SerializeField] private int strikes = 3;
    private Delivery currentOrder;
    private teaTypes teaCarrying = teaTypes.none;
    private int bitesLeft=0;


    private void Awake()
    {
        currentOrder = new Delivery();
        currentOrder.type = teaTypes.english;
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
            Debug.Log((int)currentOrder.type);
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
