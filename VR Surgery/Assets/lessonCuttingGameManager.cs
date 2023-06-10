using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class lessonCuttingGameManager : MonoBehaviour
{
    public objectsContained trayContainer;
    public Slicer slicer;
    public TextMeshProUGUI score;

    // Update is called once per frame
    void Update()
    {
        if (trayContainer.containedObjs.Count == 2)
        {
            float currScore = 100;

            //we will consider at 1 a perfect score;
            if (slicer.inaccuracy > 1)
            {
                currScore= (1 / slicer.inaccuracy) *100;
            }

            score.text = "Distance: " + slicer.inaccuracy + " | Score: " + Mathf.Round(currScore * 100f) / 100;
        }
    }


}
