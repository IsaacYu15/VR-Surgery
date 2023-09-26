using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleArms : MonoBehaviour
{

    public GameObject toggleScissors;
    public GameObject togglePinchers;

    protected OVRInput.Controller m_controller;

    // Update is called once per frame
    public void Start()
    {
        m_controller = gameObject.GetComponent<OVRGrabber>().m_controller;
    }

    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One, m_controller))
        {
            if (toggleScissors.activeSelf)
            {
                togglePinchers.SetActive(false);
                toggleScissors.SetActive(true);
            }
            else
            {
                togglePinchers.SetActive(true);
                toggleScissors.SetActive(false);
            }
        }

    }
}
