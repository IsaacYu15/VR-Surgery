using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectTouchByPlayer : MonoBehaviour
{
    public lessonGrabbingManager gameManager;
    GameObject[] cubes;
    public GameObject[] punishable;

    public float touchPunishment = 10f;
    public float touchCounter;

    public bool completed;

    public void Start()
    {
        cubes = gameManager.grabCubes;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (GameObject go in punishable)
        {
            if (collision.gameObject.name == go.name)
            {
                touchCounter += touchPunishment;

            }
        }


    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (GameObject go in punishable)
        {

             if (collision.gameObject.name == go.name)
             {
                 touchCounter += Time.deltaTime;
             }
        }


        for (int i = 0; i < cubes.Length; i++)
        {

            if (collision.gameObject.name.Equals(cubes[i].name))
            {
                completed = true;

            }
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            if (collision.gameObject.name.Equals(cubes[i].name))
            {
                completed = false;
            }
        }
    }
}
