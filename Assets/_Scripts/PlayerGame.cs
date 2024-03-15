using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerGame : MonoBehaviour
{
    [SerializeField] private int strikes = 3;
    [SerializeField] private DeliveryManager manager;
    [SerializeField] private DirectionArrow arrow;
    [SerializeField] private StaminaBar staminaBar;

    [SerializeField] public float timeForDelivery = 120;

    [SerializeField] private HighScoreTransfer highScore;
    private int teasDelivered;
    private int teasEaten;

    private Delivery currentOrder;
    private teaTypes teaCarrying = teaTypes.none;
    private int bitesLeft=0;

    [SerializeField] private float staminaDrainRate = 0.02f;
    [SerializeField] private float staminaRegainRate = 0.05f;
    [SerializeField] private float eatingStaminaRegain = 0.4f;
    [SerializeField] private float minStaminaSpeed = 0.1f;

    private float stamina = 1;

    private PlayerInput playerInput;

    private Rigidbody rb;

    public string[] teaNames = new string[5] { "Boba tea", "Green tea", "English tea", "Iced tea", "Microwave tea" };

    [SerializeField] private Image teaImageDisplay; // Assign this in the Unity Editor

    public void DisplayTeaImage(teaTypes teaType, int spriteNumber)
    {
        // Check if the teaType is none, and if so, set the Image to display no image
        if (teaType == teaTypes.none)
        {
            if (teaImageDisplay != null)
            {
                teaImageDisplay.sprite = null; // Set to no image
            }
        }
        else
        {
            // Cast the teaTypes enum to ImageName since they have the same underlying values
            ImageName imageName = (ImageName)Enum.Parse(typeof(ImageName), teaType.ToString());

            // Get the sprite from TeaImageManager
            Sprite spriteToDisplay = TeaImageManager.Instance.GetSprite(imageName, spriteNumber);

            // Set the sprite to the Image UI component
            if (spriteToDisplay != null && teaImageDisplay != null)
            {
                teaImageDisplay.sprite = spriteToDisplay;
            }
        }
    }



    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
        currentOrder = manager.createDelivery(timeForDelivery,null);
        currentOrder.location.script.enableHouse();
        playerInput=GetComponent<PlayerInput>();

        playerInput.actions["Eat"].Enable();    //Add eat tea to the eat function
        playerInput.actions["Eat"].started += eatTea;

        DialogueManager.Instance.ShowMessage("ExampleCharacter", "Order of " + teaNames[(int)currentOrder.type-1] + " to go to house");

        teasDelivered = 0;
        teasEaten = 0;
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
            stamina += Mathf.Min(staminaRegainRate * Time.deltaTime,1);
        }
        else
        {
            stamina -= staminaDrainRate * Time.deltaTime;
        }

        if(stamina<0)   //Lose if stamina hits 0
        {
            loseFunction(); 
        }

        staminaBar.setStamina(stamina);
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
        if(teaCarrying==teaTypes.none || bitesLeft == 1)  //No tea to eat
        {
            return;
        }
        bitesLeft -= 1;
        teasEaten++;
        stamina += eatingStaminaRegain;
        if(stamina>1){ stamina = 1; }   //Ensure stamina doesn't go above 1
        if(bitesLeft==0)
        {
            teaCarrying= teaTypes.none; 
        }
        DisplayTeaImage(teaCarrying, 2 - bitesLeft);
    }


    public void completeOrder(Delivery newOrder)
    {
        //Increment score

        //Order half eaten
        if(bitesLeft<2)
        {
            if(Random.Range(0.0f, 1.0f)<0.5f)   //Random number 1 in 2
            {
                Debug.Log("Order half eaten");
                strikes--;
            }
        }

        else if(currentOrder.type!=teaCarrying)  //Carrying the wrong tea
        {
            Debug.Log("Wrong order");
            //TODO: Add code for NPC telling player they're order is wrong

            strikes--;
        }
        else
        {
            teasDelivered++;
        }

        if (strikes == 0)
        {
            loseFunction();
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
        DialogueManager.Instance.ShowMessage("ExampleCharacter","Order of "+teaNames[(int)newOrder.type-1]+" to go to house");
    }

    public bool takeTea(teaTypes type)
    {
        if(teaCarrying!=teaTypes.none)  //Already have tea
        {
            return false;
        }
        teaCarrying = type;
        bitesLeft = 2;
        DisplayTeaImage(teaCarrying, 2 - bitesLeft);
        return true;
    }

    public void loseFunction()
    {
        dataToTransfer d = new dataToTransfer();
        d.teaEaten = teasEaten;
        d.deliveries = teasDelivered;
        highScore.setData(d);
        SceneManager.LoadScene("Lose Scene");
    }

    //Get speed decrease from stamina
    public float getSpeedDecrease()
    {
        return Mathf.Min(Mathf.Sqrt(stamina)+minStaminaSpeed, 1.0f);
    }
}
