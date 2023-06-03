using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceListener : MonoBehaviour
{
    public Slicer slicer;
    public Slicer slicerPerp;
    private void OnTriggerEnter(Collider other)
    {
        slicer.isTouched = true;
    }
}