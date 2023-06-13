using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectTouchByPlayer : MonoBehaviour
{
    public lessonGrabbingManager gameManager;
    public GameObject[] cubes;

    public float touchPunishment = 10f;
    public float touchCounter;

    public bool completed;

    public void Start()
    {
        cubes = gameManager.grabCubes;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.transform.root.gameObject.name == "OVRPlayerController")
        {
            touchCounter += touchPunishment;

        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.transform.root.gameObject.name == "OVRPlayerController")
        {
            touchCounter += Time.deltaTime;
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
