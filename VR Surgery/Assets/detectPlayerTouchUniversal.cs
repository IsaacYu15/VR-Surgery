using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectPlayerTouchUniversal : MonoBehaviour
{
    public GameObject[] detectPlayerComponents;
    public GameObject[] incrementScoreComponents;
    public float touchCounter = 0;
    public float score;

    private void OnTriggerStay(Collider collision)
    {

        foreach (GameObject go in detectPlayerComponents)
        {
            if (collision.gameObject == go)
            {
                touchCounter += Time.deltaTime;
                break;
            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (GameObject go in incrementScoreComponents)
        {
            if (collision.gameObject == go)
            {
                score++;
                break;
            }
        }
    }



}
