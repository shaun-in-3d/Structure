using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    public static bool operator ==( House a, House b )
    {
        return a.position == b.position && a.script==b.script;
    }

    public static bool operator!=(House a, House b)
    {
        return !(a == b);
    }

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
    public List<House> houses = new List<House>();
    private List<TeaShop> shops = new List<TeaShop>();

    private void Update()
    {
        if(!Player.tickTimer(Time.deltaTime))   //Update task timer
        {
            Player.failOrder(createDelivery(120, Player.getCurrentDelivery().location));
        }
    }

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

<<<<<<< Updated upstream
=======
    public void addTeaShop(TeaShop shop)
    {
        shops.Add(shop);
    }

    public void addTeaShop(GameObject shop,teaTypes type)
    {
        TeaShop s=new TeaShop();
        s.shop = shop;
        s.type = type;

        addTeaShop(s);
    }
    

>>>>>>> Stashed changes
    public House GetHouse(int index) 
    {
        return houses[index];
    }

    public House getHouseFunction(int index)
    {
        return houses[index];
    }

    public TeaShop GetTeaShop(int index)
    {
        return shops[index];
    }

    public Delivery createDelivery(float time, House? houseToAvoid)  //Create new delivery struct and return it, time is passed in to allow it to become either fixed, or calculated
    {
        Delivery d = new Delivery();

        int num = UnityEngine.Random.Range(0, houses.Count + (houseToAvoid==null?0:-1));  //Get next house, out of all houses except one currently at, unless house currently at is null
        if (houses[num] == houseToAvoid)
        {
            num++;
        }
        
        

        d.location = houses[num];
        d.type = (teaTypes)(UnityEngine.Random.Range(0, teaTypeCount)+1);
        d.time = time;

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
        Player.completeOrder(createDelivery(time,Player.getCurrentDelivery().location));
    }
}
