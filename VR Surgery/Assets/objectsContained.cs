using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectsContained : MonoBehaviour
{
    public List <GameObject> containedObjs = new List<GameObject>();

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.transform.root.gameObject.name != "OVRPlayerController")
        {
            containedObjs.Add(collision.gameObject);
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.transform.root.gameObject.name != "OVRPlayerController")
        {
            containedObjs.Remove(collision.gameObject);
        }

    }
}
