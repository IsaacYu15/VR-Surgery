using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Author: Isaac 
//Title: lessonCuttingGameManager

//Description: 
//This class manages / displays the score for the cutting lesson. It gets the accuracy in which the player
//cuts and displays the score. The random cut pattern & player accuracy is generated and caculated by the object to be sliced
//Detects if the game is over by seeing if the player cut the object into two pieces and placed them into the tray


public class lessonCuttingGameManager : MonoBehaviour
{
    public objectsContained trayContainer;
    public Slicer slicer;

    public TextMeshProUGUI score;

    float maxScore = 100;
    // Update is called once per frame
    void Update()
    {
        if (trayContainer.containedObjs.Count == 2)
        {
            float currScore = maxScore;

            //we will consider at 1 a perfect score;
            if (slicer.inaccuracy > 1)
            {
                currScore= (1 / slicer.inaccuracy) *100;
            }

            if (currScore < 0)
            {
                currScore = 0;
            }

            score.text = "Score: " + Mathf.Round(currScore * 100f) / 100;
        }
    }


}
