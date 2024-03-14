using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum teaTypes
{
    none,
    boba,
    green,
    english,
    iced,
    chai,
    microwave
};

public struct House
{
    public Vector2 position;
    public HouseDeliveryScript script;
};

public struct TeaShop
{
    public Vector2 position;
    public teaTypes type;
};

public struct Delivery
{
    public House location;
    public teaTypes type;
    public float time;
};


public class DeliveryManager : MonoBehaviour
{
    private const int teaTypeCount = 6;
    [SerializeField] private List<House> houses = new List<House>();
    [SerializeField] private List<TeaShop> shops = new List<TeaShop>();

    [SerializeField] private PlayerGame Player;
    public void addHouse(House house)   //called in house construction. So it can be delivered to
    {
        houses.Add(house);
    }

    public void addHouse(HouseDeliveryScript house) //Constructor with just the script, sets up the house
    {
        Vector3 position = house.gameObject.transform.position;
        House h = new House();
        h.position = new Vector2(position.x, position.z);
        h.script = house;

        addHouse(h);
    }



    private void FixedUpdate()
    {
        //Debug.Log(houses.Count);
    }


    public House GetHouse(int index) 
    {
        return houses[index];
    }

    public TeaShop GetTeaShop(int index)
    {
        return shops[index];
    }

    public Delivery createDelivery(float time)  //Create new delivery struct and return it, time is passed in to allow it to become either fixed, or calculated
    {
        Delivery d = new Delivery();
        d.location = GetHouse(Random.Range(0, houses.Count));
        d.type = (teaTypes)(Random.Range(0, teaTypeCount)+1);
        d.time = time;

        d.location.script.enableHouse();

        Debug.Log("Deliver tea "+d.type+" to house");

        return d;
    }

    public void giveTea(teaTypes type)
    {
        Player.takeTea(type);
    }

    public void completeDelivery()  //Complete delivery, to be called in house
    {
        const float time = 120; //Delay
        Player.completeOrder(createDelivery(time));
    }
}
