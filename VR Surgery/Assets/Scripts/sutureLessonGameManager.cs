using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sutureLessonGameManager : MonoBehaviour
{
    public TextMeshProUGUI maxNeedleVelocity;
    public TextMeshProUGUI scoreText;
    public GameObject suture;
    public GameObject[] surgicalMeshes;
    public GameObject[] surgicalRings;

    float score = 100;
    float maxVelocity;

    float velocityPunishment = 0;
    float touchPunishment = 0;

    float duration = 0;
    bool gameOver;
    bool pullIn = false;

    sutureInBounds sutureBounds;
    Rope rope;

    Vector3 midpoint;

    // Start is called before the first frame update
    void Start()
    {

        maxVelocity = Mathf.Round(Random.Range(0.3f, 1f) * 10) / 10;
        maxNeedleVelocity.text = "Max Velocity: " + maxVelocity + " m/s";
        sutureBounds = suture.GetComponent<sutureInBounds>();
        rope = suture.GetComponent<Rope>();

        foreach (GameObject go in surgicalMeshes)
        {
            midpoint += go.transform.position;
        }

        midpoint /= 2;
    }


    public bool allRingsHit()
    {

        List<GameObject> t_rings = new List<GameObject>();

        for (int i = 0; i < surgicalRings.Length; i ++)
        {
            t_rings.Add(surgicalRings[i]);
        }

        for (int i = 0; i < rope.ropePositions.Count; i ++)
        {
            Collider[] hitColliders = Physics.OverlapSphere(rope.ropePositions[i].position, 0.01f);

            foreach (Collider hitCollider in hitColliders)
            {
                for (int j = 0; j < t_rings.Count; j++)
                {
                    if (hitCollider.transform.gameObject == t_rings[j])
                    {
                        t_rings.Remove(t_rings[j]);
                    }
                }
            }

        }

        if (t_rings.Count == 0)
        {
            return true;
        } else
        {
            return false;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            //game end
            int passedThroughs = 0;

            foreach (GameObject go in surgicalRings)
            {
                if (go.GetComponent<changeColourWhenTouched>().passedThrough)
                {
                    passedThroughs++;
                }
            }

            //score counter
            score = 100;

            if (sutureBounds.velocity.magnitude > maxVelocity && sutureBounds.colInMesh)
            {
                velocityPunishment -= Mathf.Abs(sutureBounds.velocity.magnitude - maxVelocity);
            }

            touchPunishment = 0;
            foreach (GameObject go in surgicalMeshes)
            {
                touchPunishment -= go.GetComponent<detectPlayerTouchUniversal>().touchCounter;
            }

            score = 100 + touchPunishment * 2f + velocityPunishment * 5f;
            scoreText.text = "SCORE: " + Mathf.Round(score * 100) / 100;

            if (passedThroughs == surgicalRings.Length & !suture.GetComponent<sutureInBounds>().colInMesh & !suture.GetComponent<sutureInBounds>().wasIn && allRingsHit())
            {
                pullIn = true;
            }

            if (pullIn) { 
                if (suture.GetComponent<sutureInBounds>().velocity.magnitude > 0.2 && suture.transform.parent != null)
                {
                    foreach (GameObject go in surgicalMeshes)
                    {
                        duration += suture.GetComponent<sutureInBounds>().velocity.magnitude;
                        go.transform.position = Vector3.Lerp(go.transform.position, midpoint, duration / 100000);

                        if (go.GetComponent<detectPlayerTouchUniversal>().score > 0)
                        {
                            gameOver = true;
                        }


                        for (int i = 0; i < surgicalRings.Length; i++)
                        {
                            surgicalRings[i].SetActive(false);
                        }

                    }
                }

            }

        } else
        {
            scoreText.text = "FINAL SCORE: " + Mathf.Round(score * 100) / 100;
        }



    }

}
