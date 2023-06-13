using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColourWhenTouched : MonoBehaviour
{
    public Color touched;
    public Color normal;
    public bool passedThrough;

    public void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = normal;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Suture")
        {
            gameObject.GetComponent<MeshRenderer>().material.color = touched;
            passedThrough = true;
        }

    }

}
