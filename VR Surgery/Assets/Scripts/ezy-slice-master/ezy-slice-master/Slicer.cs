using UnityEngine;
using EzySlice;
public class Slicer : MonoBehaviour
{
    public Material materialAfterSlice;
    public LayerMask sliceMask;
    public bool isTouched;
    public float sliceAmount;

    GameObject prevLowerHull;
    GameObject prevUpperHull;

    private void Update()
    {
        if (isTouched == true)
        {
            isTouched = false;

            Collider[] objectsToBeSliced = Physics.OverlapBox(transform.position, new Vector3(1, 0.1f, 0.1f), transform.rotation, sliceMask);

            foreach (Collider objectToBeSliced in objectsToBeSliced)
            {
                SlicedHull slicedObject = SliceObject(objectToBeSliced.gameObject, materialAfterSlice);

                GameObject upperHullGameobject = slicedObject.CreateUpperHull(objectToBeSliced.gameObject, materialAfterSlice);
                GameObject lowerHullGameobject = slicedObject.CreateLowerHull(objectToBeSliced.gameObject, materialAfterSlice);

                Transform slicedTransform = objectToBeSliced.gameObject.transform;

                Vector3 newSliceScale = new Vector3(slicedTransform.localScale.x, slicedTransform.localScale.y, sliceAmount);
                upperHullGameobject.transform.localScale = newSliceScale;
                lowerHullGameobject.transform.localScale = newSliceScale;

                Vector3 direction = new Vector3(0, 0, -1);
                upperHullGameobject.transform.position = slicedTransform.position + direction * sliceAmount / 2; //FIGURE OUT WHY ITS DOING DAT HALF THING
                lowerHullGameobject.transform.position = slicedTransform.position + direction * sliceAmount / 2;


                sliceAmount += 0.025f; //add a little offset in the slicing
                GameObject remainingToBeSliced = Instantiate(objectToBeSliced.gameObject, objectToBeSliced.gameObject.transform.position - direction * sliceAmount / 2, objectToBeSliced.gameObject.transform.rotation);
                newSliceScale = new Vector3(slicedTransform.localScale.x, slicedTransform.localScale.y, slicedTransform.localScale.z - sliceAmount);
                remainingToBeSliced.transform.localScale = newSliceScale;

                MakeItPhysical(upperHullGameobject, remainingToBeSliced, direction);
                MakeItPhysical(lowerHullGameobject, remainingToBeSliced, direction);

                if (prevLowerHull != null && prevUpperHull != null)
                {
                    prevUpperHull.GetComponent<HingeJoint>().connectedBody = upperHullGameobject.GetComponent<Rigidbody>();
                    prevLowerHull.GetComponent<HingeJoint>().connectedBody = lowerHullGameobject.GetComponent<Rigidbody>();
                } 

                prevLowerHull = lowerHullGameobject;
                prevUpperHull = upperHullGameobject;



                Destroy(objectToBeSliced.gameObject);
            }
        }
    }

    private void MakeItPhysical(GameObject obj, GameObject connectedTo, Vector3 dir)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
        HingeJoint j = obj.AddComponent<HingeJoint>();
        j.connectedBody = connectedTo.GetComponent<Rigidbody>();
        j.autoConfigureConnectedAnchor = false;
        j.connectedAnchor = dir * sliceAmount;
        j.anchor -= dir * sliceAmount / 2;



    }

    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        return obj.Slice(transform.position, transform.up, crossSectionMaterial);
    }


}