using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleMesh : MonoBehaviour
{
    /// <summary>
    /// http://blog.nick.je/endless-procedural-controlPtd-mesh-generation-in-unity-part-1/
    /// </summary>

    public List<Vector3> controlPtLeft = new List<Vector3>();
    public List<Vector3> controlPtRight = new List<Vector3>();

    //MeshProperties
    public Mesh mesh;
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public int resolution = 20;

    /// <summary>
    /// どちらも　4+(n*3)の数ないといけない
    /// </summary>
    /// <param name="ControlPtLeft"></param>
    /// <param name="ControlPtRight"></param>
    public  BottleMesh(MeshFilter filter)
    {
        // Get a reference to the mesh and clear it
        mesh = filter.mesh;
        mesh.Clear();
    }

    public void CreateMesh(List<Vector3> ControlPtLeft, List<Vector3> ControlPtRight)
    {
        controlPtLeft = ControlPtLeft;
        controlPtRight = ControlPtRight;

        mesh.Clear();
        vertices.Clear();
        triangles.Clear();

        CalculateBezierCurve(controlPtLeft, resolution);
        CalculateBezierCurve(controlPtRight, resolution);
        
        var leftLengh = vertices.Count / 2;
        for (int i = 0; i < leftLengh-1; i++)
        {
            triangles.Add(i);
            triangles.Add(i + leftLengh);
            triangles.Add(i + leftLengh + 1);
            triangles.Add(i);
            triangles.Add(i + leftLengh + 1);
            triangles.Add(i + 1);
        }

        // Assign the vertices and triangles to the mesh 
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    void CalculateBezierCurve(List<Vector3> controlPts, int resolution)
    {
        for(int i = 0; i < (int)((controlPts.Count - 1) / 3); i++)
        {
            // Number of points to draw, how smooth the controlPt is
            for (int j = 0; j < resolution; j++)
            {
                float t = (float)j / (float)(resolution - 1);
                // Get the point on our controlPt using the points generated above
                Vector3 tempPt = CalculateBezierPoint(t, controlPts[i *3], controlPts[i * 3 + 1], controlPts[i * 3 + 2], controlPts[i * 3 + 3]);
                vertices.Add(tempPt);
            }
        }
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
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
