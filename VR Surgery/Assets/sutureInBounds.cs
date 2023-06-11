using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sutureInBounds : MonoBehaviour
{
    public GameObject colliderContainer;
    public GameObject refTip;

    public void Update()
    {
        bool partialInMesh = false;

        foreach (Transform go in colliderContainer.transform)
        {
            if (go.gameObject.GetComponent<inSurgicalMesh>().inMesh)
            {
                partialInMesh = true;
                break;
            }
        }

        if (partialInMesh && refTip.GetComponent<inSurgicalMesh>().inMesh == false)
        {

        }

    }
}
