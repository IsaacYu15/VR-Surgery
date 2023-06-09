using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class returnToMenuButton : MonoBehaviour
{
    public float currTime = 0;
    public float maxHoldTime = 2f;
    public string sceneName;

    public Slider slider;

    bool held;

    public void Start()
    {
        slider.maxValue = maxHoldTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        held = true;
    }

    public void OnTriggerExit(Collider other)
    {
        held = false;
    }

    public void Update()
    {

        if (held)
        {
            currTime += Time.deltaTime;
        } else
        {
            currTime = 0;
        }

        if (currTime >= maxHoldTime)
        {
            SceneManager.LoadScene(sceneName);
        }

        slider.value = currTime;
    }
}
