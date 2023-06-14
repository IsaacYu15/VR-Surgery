using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Author: Isaac 
//Title: lessonGrabbingManager

//Description: 
//This class manages the score for the grabbing lesson. It sees if all the containers have a
//box inside of them and deducts points if the user touches the box
//Ends the game when all the boxes have been placed inside the container

public class lessonGrabbingManager : MonoBehaviour
{
    public TextMeshProUGUI scoreCounter;

    public GameObject grabCubeParent;
    public GameObject grabContainerParent;

    public GameObject [] grabCubes;
    public GameObject [] grabContainers;

    float maxScore = 100;
    float currScore = 0;

    public void Awake()
    {
        currScore = maxScore;

    }

    public void Update()
    {
        if (isGameCompleted())
        {
            for (int i = 0; i < grabCubes.Length; i ++)
            {
                grabCubes[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }

            scoreCounter.text = "FINAL SCORE: " + Mathf.Round(currScore * 100f) / 100f;

        } else
        {
            currScore = maxScore;

            for (int i = 0; i < grabContainers.Length; i ++)
            {
                currScore -= grabContainers[i].GetComponent<detectTouchByPlayer>().touchCounter;
            }

            if (currScore < 0)
            {
                currScore = 0;
            }

            scoreCounter.text = "Score: " + Mathf.Round(currScore * 100f) / 100f;
        }


    }

    public bool isGameCompleted()
    {
        int completedItems = 0;

        for (int i = 0; i < grabContainers.Length; i++)
        {
            detectTouchByPlayer playerTouch = grabContainers[i].GetComponentInChildren<detectTouchByPlayer>();

            if (playerTouch.completed)
            {
                completedItems++;
            }
        }

        if (completedItems == grabContainers.Length)
        {
            return true;
        } else
        {
            return false;
        }

    }


}
