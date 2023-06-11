using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inSurgicalMesh : MonoBehaviour
{
    public LayerMask surgicalMesh;
    public bool inMesh;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Mathf.Log(surgicalMesh.value, 2))
        {
            inMesh = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Mathf.Log(surgicalMesh.value, 2))
        {
            inMesh = false;
        }
    }
}
