using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        grabCubes = getChildren(grabCubeParent.transform);
        grabContainers = getChildren(grabContainerParent.transform);

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
            currScore -= Time.deltaTime * 0.25f;

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


    public GameObject [] getChildren (Transform parent)
    {
        GameObject [] children = new GameObject[parent.childCount];

        int i = 0;

        foreach (Transform child in parent)
        {
            children[i] = child.gameObject;
            i++;
        }

        return children;
    }
}
