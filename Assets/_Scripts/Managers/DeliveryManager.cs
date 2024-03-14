using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private List<House> houses;
    [SerializeField] private List<TeaShop> shops;

    [SerializeField] private PlayerGame Player;

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
        //d.location = GetHouse(Random.Range(0, houses.Count));
        //d.type = (teaTypes)(Random.Range(0, teaTypeCount)+1);
        d.type = teaTypes.english;
       // d.time = time;

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
