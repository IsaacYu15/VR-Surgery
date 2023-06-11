using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColourWhenTouched : MonoBehaviour
{
    public Color touched;
    public Color normal;

    public void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = normal;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Suture")
        {
            gameObject.GetComponent<MeshRenderer>().material.color = touched;
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "Suture")
        {
            gameObject.GetComponent<MeshRenderer>().material.color = normal;
        }

    }
}
