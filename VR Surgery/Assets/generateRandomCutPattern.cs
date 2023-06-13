using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateRandomCutPattern : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject sphere;
    public Material patternMat;
    public Material playerMat;

    public int numberChangesInDirection;

    public List<Vector3> pointsToCut = new List<Vector3>();
    LineRenderer lineRender;

    float boundsXend;
    float boundsXstart;
    float boundsZend;
    float boundsZstart;
    float cutlength;
    float currZvalue;

    public float inaccuracyDistance;

    void Start()
    {
        if (pointsToCut.Count == 0)
        {
            boundsXend = transform.position.x + transform.localScale.x / 2.5f; //leave buffer room at the edge of the sliced ob
            boundsXstart = transform.position.x - transform.localScale.x / 2.5f;

            boundsZend = transform.position.z + transform.localScale.z / 2f;
            boundsZstart = transform.position.z - transform.localScale.z / 2f;

            cutlength = (Mathf.Abs(boundsZend - boundsZstart)) / numberChangesInDirection;
            currZvalue = boundsZstart;

            lineRender = gameObject.GetComponent<LineRenderer>();

            generatePattern();
        }

    }

    public void generatePattern ()
    {
        lineRender.positionCount = numberChangesInDirection + 1;

        pointsToCut.Add(new Vector3((boundsXend + boundsXstart)/2 , transform.position.y, boundsZstart));
        lineRender.SetPosition(0, pointsToCut[0]);

        for (int i = 1; i < numberChangesInDirection + 1; i ++)
        {
            currZvalue += cutlength;
            pointsToCut.Add(new Vector3(Random.Range (boundsXend , boundsXstart), transform.position.y, currZvalue));
            lineRender.SetPosition(i, pointsToCut[i]);
        }
    }

    public void getPoints (List<Vector3> player)
    {
        boundsZend = lineRender.GetPosition(lineRender.positionCount - 1).z;
        boundsZstart = lineRender.GetPosition(0).z;

        float trackingZ = boundsZstart;
        float step = (Mathf.Abs(boundsZend - boundsZstart)) / 100f; //make 100 comparisons between the lines

        int patternIndex = 0;
        int playerIndex = 0;

        float patternX = pointsToCut[0].x;
        float playerX = player[0].x;

        for (int i = 0; i < 100; i++)
        {
            if (trackingZ - step > pointsToCut[patternIndex + 1].z && patternIndex + 1 < pointsToCut.Count - 1)
            {
                patternIndex++;
            }

            if (trackingZ - step > player[playerIndex + 1].z && playerIndex + 1 < player.Count - 1)
            {
                playerIndex++;
            }

            GameObject pat = Instantiate(sphere, new Vector3(patternX, transform.position.y, trackingZ), Quaternion.identity);
            pat.GetComponent<MeshRenderer>().material = patternMat;
            GameObject pla = Instantiate(sphere, new Vector3(playerX, transform.position.y, trackingZ), Quaternion.identity);
            pla.GetComponent<MeshRenderer>().material = playerMat;


            float patternSlope = (pointsToCut[patternIndex].x - pointsToCut[patternIndex + 1].x) / (pointsToCut[patternIndex].z - pointsToCut[patternIndex + 1].z) * step;
            float playerSlope = (player[playerIndex].x - player[playerIndex + 1].x) / (player[playerIndex].z - player[playerIndex + 1].z) * step;

            patternX += patternSlope;
            playerX += playerSlope;

            inaccuracyDistance += Mathf.Abs(patternX - playerX);

            trackingZ += step;

        }

        Debug.Log(inaccuracyDistance);

    }
}
