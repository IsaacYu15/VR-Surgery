using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateTowardsHand : MonoBehaviour
{

    public Transform target;


    void Update()
    {
        transform.localScale = new Vector3 (transform.localScale.x, Vector3.Distance(transform.position, target.position), transform.localScale.z);
        Vector3 targetDirection = target.position - transform.position;

        Quaternion orientation = Quaternion.FromToRotation(Vector3.up, targetDirection);
        transform.rotation = orientation; 
    }
}
