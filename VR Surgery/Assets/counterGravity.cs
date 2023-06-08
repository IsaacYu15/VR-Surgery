using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counterGravity : MonoBehaviour
{
    Rigidbody rb;
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();   
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
    }
}
