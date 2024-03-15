using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayHighScore : MonoBehaviour
{
    private HighScoreTransfer highScore;

    private void Awake()
    {
        highScore = GameObject.Find("HighScore").GetComponent<HighScoreTransfer>();
        dataToTransfer d=highScore.getData();

        GetComponent<TextMeshProUGUI>().text = "You scored" + d.deliveries + " deliveries and ate " + d.teaEaten+ "teas!";
    }
}
