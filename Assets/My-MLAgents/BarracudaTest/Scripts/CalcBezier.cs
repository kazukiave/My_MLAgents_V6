using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcBezier : MonoBehaviour
{

    public GameObject[] controlPts = new GameObject[4];

    List<Vector3> vertices = new List<Vector3>();
    List<GameObject> verticesPts = new List<GameObject>();
    LineRenderer curve;
    // Start is called before the first frame update
    void Start()
    {
        curve = gameObject.AddComponent<LineRenderer>();
        curve.material = new Material(Shader.Find("Sprites/Default"));
        curve.widthMultiplier = 0.2f;
  
    }

    // Update is called once per frame
    void Update()
    {

        var controlVecs = new Vector3[4];
        vertices.Clear();

        for(int i = 0; i < verticesPts.Count; i++)
        {
            Destroy(verticesPts[i]);
        }
        verticesPts.Clear();

        for (int i = 0; i < controlPts.Length; i++)
        {
            controlVecs[i] = controlPts[i].transform.position;
        }

        int resolution = 20;
        for (int i = 0; i < resolution; i++)
        {
            float t = (float)i / (float)(resolution - 1);
            // Get the point on our controlPt using the points generated above
            Vector3 p = CalculateBezierPoint(t, controlVecs[0], controlVecs[1], controlVecs[2], controlVecs[3]);
            vertices.Add(p);
        }
        curve.positionCount = vertices.Count;
        for (int i = 0; i < vertices.Count; i++)
        {
            curve.SetPosition(i, vertices[i]);
            var tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempSphere.transform.position = vertices[i];
            tempSphere.transform.localScale = Vector3.one* 0.1f;
            verticesPts.Add(tempSphere);
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
