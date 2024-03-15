using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGame : MonoBehaviour
{
    [SerializeField] private int strikes = 3;
    [SerializeField] private DeliveryManager manager;
    [SerializeField] private DirectionArrow arrow;

    private Delivery currentOrder;
    private teaTypes teaCarrying = teaTypes.none;
    private int bitesLeft=0;

    [SerializeField] private float staminaDrainRate=0.02f;
    [SerializeField] private float staminaRegainRate = 0.05f;
    [SerializeField] private float eatingStaminaRegain = 0.4f;
    private float stamina = 1;

    private PlayerInput playerInput;

    Rigidbody rb;


    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
        currentOrder = manager.createDelivery(5,null);
        currentOrder.location.script.enableHouse();
        playerInput=GetComponent<PlayerInput>();

        playerInput.actions["Eat"].Enable();    //Add eat tea to the eat function
        playerInput.actions["Eat"].started += eatTea;   
    }

    private void Update()
    {
        //Update arrow
        if(teaCarrying==currentOrder.type)
        {
            arrow.targetObject = currentOrder.location.script.gameObject.transform;
        }
        else
        {
            arrow.targetObject = manager.GetTeaShop(currentOrder.type).shop.transform;
        }

        //Update stamina
        if(rb.velocity.sqrMagnitude<=0.02f)
        {
            stamina += staminaRegainRate * Time.deltaTime;
        }
        else
        {
            stamina -= staminaDrainRate * Time.deltaTime;
        }

        if(stamina<0)   //Lose if stamina hits 0
        {
            loseFunction(); 
        }
        
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
    

    public void eatTea(InputAction.CallbackContext context)
    {
        if(teaCarrying==teaTypes.none)  //No tea to eat
        {
            return;
        }

        bitesLeft -= 1;
        stamina += eatingStaminaRegain;
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
                loseFunction();
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
            loseFunction();
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

    public void loseFunction()
    {
        //TODO: Add code for losing
        Debug.Log("You lose!");
    }
}
