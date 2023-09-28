using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform playerTip;
    public Transform ropeSpawn;
    public Transform firstPos;

    public LineRenderer rope;
    public LayerMask collMask;

    public List<Transform> ropePositions = new List<Transform>();

    private void Awake()
    {
        AddPosToRope(firstPos);
    }

    private void Update()
    {
        UpdateRopePositions();
        LastSegmentGoToPlayerPos();

        DetectCollisionEnter();
        if (ropePositions.Count > 2) DetectCollisionExits();
    }

    private void DetectCollisionEnter()
    {
        RaycastHit hit;
        if (Physics.Linecast(playerTip.position, rope.GetPosition(ropePositions.Count - 2), out hit, collMask))
        {

            if (Vector3.Distance (hit.point, ropePositions[ropePositions.Count-1].position) > 1) {

                ropePositions.RemoveAt(ropePositions.Count - 1);

                GameObject go = new GameObject();
                go.transform.position = hit.point;
                go.transform.parent = hit.transform.root.transform;
                AddPosToRope(go.transform);
            }

        }
    }

    private void DetectCollisionExits()
    {
        RaycastHit hit;
        if (!Physics.Linecast(playerTip.position, rope.GetPosition(ropePositions.Count - 3), out hit, collMask))
        {
            ropePositions.RemoveAt(ropePositions.Count - 2);
        }
    }

    private void AddPosToRope(Transform _pos)
    {
        ropePositions.Add(_pos);
        ropePositions.Add(ropeSpawn); //Always the last pos must be the player
    }

    private void UpdateRopePositions()
    {
        rope.positionCount = ropePositions.Count;

        for (int i =0;i < rope.positionCount; i ++)
        {
            rope.SetPosition(i, ropePositions[i].position);
        }

    }

    private void LastSegmentGoToPlayerPos()
    {
        rope.SetPosition(rope.positionCount - 1, playerTip.position);
    }
}