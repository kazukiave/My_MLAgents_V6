using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMeshTest : MonoBehaviour
{
    public int resolution = 20;

    List<GameObject> vtxPos = new List<GameObject>();
    Mesh mesh;
    private int sphereNum = 10;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        var filter = gameObject.GetComponent<MeshFilter>();
        mesh = filter.mesh;
        mesh.Clear();

        for(int i = 0; i < sphereNum; i++)
        {
            var tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempSphere.transform.position = new Vector3(0, i);
            vtxPos.Add(tempSphere);
        }

        for(int i = 0; i < sphereNum; i++)
        {
            var tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempSphere.transform.position = new Vector3(5, i);
            vtxPos.Add(tempSphere);
        }
    }


    // Update is called once per frame
    void Update()
    {
        /*
        frameCount++;
        if(frameCount > 10)
        {
            frameCount = 0;
        }
        else
        {
            return;
        }
        */
        if (!Input.GetKeyDown(KeyCode.Space)) return;


        vertices.Clear();
        triangles.Clear();

        foreach (var vtx in vtxPos)
        {
            vertices.Add(vtx.transform.position);
        }

        for (int i = 0; i < sphereNum -1 ; i++)
        {
            triangles.Add(i);
            triangles.Add(i + sphereNum);
            triangles.Add(i + sphereNum + 1);
            triangles.Add(i);
            triangles.Add(i + sphereNum + 1);
            triangles.Add(i + 1);
        }


        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();


        for (int i = 0; i < resolution; i++)
        {
            float t = (float)i / (float)(resolution - 1);
            // Get the point on our curve using the points generated above
            Vector3 p = CalculateBezierPoint(t, vertices[0], vertices[1], vertices[2], vertices[3]);
            var tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempSphere.transform.position = p;
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}

