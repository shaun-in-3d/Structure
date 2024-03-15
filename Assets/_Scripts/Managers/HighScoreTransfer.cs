using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct dataToTransfer
{
    public int deliveries;
    public int teaEaten;
};
public class HighScoreTransfer : MonoBehaviour
{
    public static HighScoreTransfer instance;

    private dataToTransfer data;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void setData(dataToTransfer d)
    {
        data = d;
    }

    public dataToTransfer getData()
    {
        return data;
    }
}
