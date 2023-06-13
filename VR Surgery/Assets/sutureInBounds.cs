using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sutureInBounds : MonoBehaviour
{

    public TextMeshProUGUI currentNeedleVelocity;

    public GameObject colliderContainer;
    public GameObject refTip;

    public bool colInMesh;
    public bool wasIn = false;

    Vector3 lastPosition;
    public Vector3 velocity;

    public void Start()
    {
        lastPosition = velocity;
    }


    public void Update()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        currentNeedleVelocity.text = "Suture Velocity: " + Mathf.Round(velocity.magnitude * 10)/10 + " m/s ";

        if (refTip.GetComponent<inSurgicalMesh>().inMesh)
        {
            wasIn = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        } else
        {
            colInMesh = false;

            foreach (Transform child in colliderContainer.transform)
            {
                if (child.gameObject.GetComponent<inSurgicalMesh>().inMesh)
                {
                    colInMesh = true;
                    break;
                }
            }

            if (!colInMesh && wasIn)
            {
                wasIn = false;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }

        }



    }


}
