using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLocation : MonoBehaviour
{
    public Transform relocatePos;
    public Transform itemToRelocate;

    // Start is called before the first frame update
    void Start()
    {
        itemToRelocate.position = relocatePos.position;
    }

}
