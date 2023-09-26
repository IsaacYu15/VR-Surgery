using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBody : MonoBehaviour
{
    public bool lockInPosition;
    Vector3 intialPos;

    public float intensityForce = 1f;
    public float mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;

    private Mesh OriginalMesh, MeshClone;
    private MeshRenderer renderer;
    private JellyVertex[] vertex;
    private Vector3[] vertexArrays;

    void Start()
    {
        intialPos = transform.position;

        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;
        renderer = GetComponent<MeshRenderer>();

        vertex = new JellyVertex[MeshClone.vertices.Length];
        for (int i =0;i<MeshClone.vertices.Length; i ++)
        {
            vertex[i] = new JellyVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position != intialPos && lockInPosition)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().position =  intialPos;
        } else
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;

            vertexArrays = OriginalMesh.vertices;

            for (int i = 0; i < vertex.Length; i++)
            {
                Vector3 target = transform.TransformPoint(vertexArrays[vertex[i].ID]);
                float intensity = (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * intensityForce;
                vertex[i].Shake(target, mass, stiffness, damping);
                target = transform.InverseTransformPoint(vertex[i].position);
                vertexArrays[vertex[i].ID] = Vector3.Lerp(vertexArrays[vertex[i].ID], target, intensity);
            }
            MeshClone.vertices = vertexArrays;
        }

    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 position;
        public Vector3 velocity, force;

        public JellyVertex (int id, Vector3 pos)
        {
            ID = id;
            position = pos;
        }

        public void Shake (Vector3 target, float m, float s, float d)
        {
            force = (target - position) * s;
            velocity = (velocity + force / m) * d;
            position += velocity;

            if ((velocity+force + force/m).magnitude < 0.001f)
            {
                position = target;
            }
        }
    }
}
