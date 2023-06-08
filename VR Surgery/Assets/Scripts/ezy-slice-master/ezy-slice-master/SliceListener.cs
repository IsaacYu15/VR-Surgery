using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceListener : MonoBehaviour
{
    public Slicer slicer;

    private void OnTriggerEnter(Collider other)
    {
        slicer.isTouched = true;
    }
}