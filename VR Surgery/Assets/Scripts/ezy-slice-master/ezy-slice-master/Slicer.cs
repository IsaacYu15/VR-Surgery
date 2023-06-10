using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
public class Slicer : MonoBehaviour
{
    public bool isPressed;
    public Material materialAfterSlice;
    public LayerMask sliceMask;
    public bool isTouched;
    public float sliceAmount;

    public GameObject parentLower;
    public GameObject parentUpper;
    public GameObject refTip;
    public Color [] tints;

    public List<Vector3> allCutPoints = new List<Vector3>();
    public float inaccuracy;

    private void Update()
    {

        if (isTouched == true && isPressed)
        {
            isTouched = false;

            Collider[] objectsToBeSliced = Physics.OverlapBox(transform.position, new Vector3(1, 0.1f, 0.1f), transform.rotation, sliceMask);

            foreach (Collider objectToBeSliced in objectsToBeSliced)
            {
                //player to sliceable object directions
                Vector3 direction = new Vector3(0, 0, -1);

                //sliced objects
                SlicedHull slicedObject = SliceObject(objectToBeSliced.gameObject, materialAfterSlice);
                GameObject upperHullGameobject = slicedObject.CreateUpperHull(objectToBeSliced.gameObject, materialAfterSlice);
                GameObject lowerHullGameobject = slicedObject.CreateLowerHull(objectToBeSliced.gameObject, materialAfterSlice);
                Transform slicedTransform = objectToBeSliced.gameObject.transform;

                //slice amount
                Vector3 edgeOfSliced = slicedTransform.position + direction * slicedTransform.localScale.z / 2;
                sliceAmount = Mathf.Abs(refTip.transform.position.z - edgeOfSliced.z);

                allCutPoints.Add(refTip.transform.position);

                bool breakCase = (slicedTransform.localScale.z - sliceAmount < 0);

                //determine if we should allow the user to keep cutting the object, or remove it as a whole
                if (!breakCase)
                {
                    Vector3 newSliceScale = new Vector3(slicedTransform.localScale.x, slicedTransform.localScale.y, sliceAmount);
                    upperHullGameobject.transform.localScale = newSliceScale;
                    lowerHullGameobject.transform.localScale = newSliceScale;

                    upperHullGameobject.transform.position = slicedTransform.position + direction * (slicedTransform.localScale.z - sliceAmount) / 2;
                    lowerHullGameobject.transform.position = slicedTransform.position + direction * (slicedTransform.localScale.z - sliceAmount) / 2;

                    Vector3 newPos = objectToBeSliced.gameObject.transform.position - direction * sliceAmount / 2;
                    newSliceScale = new Vector3(slicedTransform.localScale.x, slicedTransform.localScale.y, slicedTransform.localScale.z - sliceAmount);
                    slicedTransform.position = newPos;
                    slicedTransform.localScale = newSliceScale;

                }

                //parent all lower and upper hulls together
                if (parentLower == null || parentUpper == null)
                {
                    parentLower = lowerHullGameobject;
                    parentUpper = upperHullGameobject;
                    parentLower.gameObject.name += ("PARENT");
                    parentUpper.gameObject.name += ("PARENT");
                }
                else
                {
                    lowerHullGameobject.transform.parent = parentLower.transform;
                    upperHullGameobject.transform.parent = parentUpper.transform;
                }



                MakeItPhysical(upperHullGameobject);
                MakeItPhysical(lowerHullGameobject);

                Color lowerColor = tints[Random.Range(0, tints.Length)];
                Color upperColor = tints[Random.Range(0, tints.Length)];
                upperHullGameobject.GetComponent<MeshRenderer>().material.color = upperColor;

                while (upperColor.Equals(lowerColor))
                {
                    lowerColor = tints[Random.Range(0, tints.Length)];
                }

                lowerHullGameobject.GetComponent<MeshRenderer>().material.color = lowerColor;

                //merge to one mesh
                if (breakCase)
                {
                    slicedTransform.gameObject.GetComponent<generateRandomCutPattern>().getPoints(allCutPoints);
                    inaccuracy = slicedTransform.gameObject.GetComponent<generateRandomCutPattern>().inaccuracyDistance;

                    combineMeshChildren(parentLower.transform);
                    combineMeshChildren(parentUpper.transform);
                    Destroy(slicedTransform.gameObject);
                }



                isPressed = false;
                break;

            }
        }
    }

    public void combineMeshChildren (Transform parent) {
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            Mesh mShared = meshFilters[i].sharedMesh;

            combine[i].mesh = mShared;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            if (mShared.subMeshCount > 1)
            {
                // combine submeshes
                for (int j = 1; j < mShared.subMeshCount; j++)
                {
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = mShared;
                    ci.subMeshIndex = j;
                    ci.transform = meshFilters[i].transform.localToWorldMatrix;
                }
            }
        }
        
        //new merged mesh -> add the right properties
        GameObject go = new GameObject(parent.gameObject.name + "_Merged");

        go.AddComponent<MeshFilter>();
        MeshFilter goFilter = go.GetComponent<MeshFilter>();
        goFilter.mesh = new Mesh();
        goFilter.mesh.CombineMeshes(combine); //combine func

        go.AddComponent<MeshRenderer>();
        go.AddComponent<BoxCollider>();
        /*
        go.AddComponent<MeshCollider>();
        MeshCollider meshCol = go.GetComponent<MeshCollider>();
        meshCol.convex = enabled;
        meshCol.sharedMesh = goFilter.sharedMesh;
        */

        go.GetComponent<MeshRenderer>().material = parent.GetComponent<MeshRenderer>().material;
        go.GetComponent<MeshRenderer>().material.color = Color.white;
        go.AddComponent<Rigidbody>();

        go.AddComponent<OVRGrabbable>();
        go.GetComponent<OVRGrabbable>().enabled = true;

        Destroy(parent.gameObject);
    }

    private void MakeItPhysical(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        return obj.Slice(transform.position, transform.up, crossSectionMaterial);
    }


}