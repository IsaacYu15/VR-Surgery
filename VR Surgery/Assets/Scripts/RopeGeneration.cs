using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGeneration : MonoBehaviour
{
    public Transform ropeSpawnLocation;
    public GameObject ropePointPref;
    public int numberPoints;
    public float pointSpacing;

    public GameObject[] ropePoints;

    public LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Start()
    {

        ropePoints = new GameObject[numberPoints];

        Vector3 currPos = ropeSpawnLocation.position;

        for (int i = 0; i < numberPoints; i ++)
        {
            currPos += new Vector3(0, -pointSpacing, 0);

            GameObject go = Instantiate(ropePointPref, currPos, Quaternion.identity);
            go.transform.parent = transform;

            ropePoints[i] = go;
        }

        ropePoints[0].GetComponent<ConfigurableJoint>().connectedBody = ropePoints[1].GetComponent<Rigidbody>();
        ropePoints[0].transform.position = ropeSpawnLocation.position;
        ropePoints[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        

        for (int i = 1; i < numberPoints; i ++)
        {
           ropePoints[i].GetComponent<ConfigurableJoint>().connectedBody = ropePoints[i - 1].GetComponent<Rigidbody>();
        }

        lineRenderer.positionCount = numberPoints;

    }

    // Update is called once per frame


    float offset;

    void Update()
    {

        ropePoints[0].transform.position = ropeSpawnLocation.position;

        offset = Vector3.Distance(ropePoints[0].transform.position, ropePoints[1].transform.position);

        for (int i = 0; i < ropePoints.Length; i ++)
        {
            lineRenderer.SetPosition(i, ropePoints[i].transform.position);
            
        }

    }

    public void LateUpdate()
    {
        for (int i = 1; i < ropePoints.Length; i ++)
        {
            ropePoints[i].GetComponent<Rigidbody>().velocity = Vector3.zero;

            ropePoints[i].GetComponent<Rigidbody>().transform.localPosition -= (ropePoints[i].GetComponent<Rigidbody>().transform.localPosition - ropePoints[i - 1].GetComponent<Rigidbody>().transform.localPosition).normalized * (offset - pointSpacing);
        }
    }
}
