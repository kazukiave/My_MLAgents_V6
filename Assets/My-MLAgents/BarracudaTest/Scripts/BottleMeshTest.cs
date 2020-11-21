using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleMeshTest : MonoBehaviour
{
    public Material mat;

    BottleMesh bottleMesh;
    List<GameObject> leftControlePts = new List<GameObject>();
    List<GameObject> rightControlePts = new List<GameObject>();
    List<Vector3> leftControleVec = new List<Vector3>();
    List<Vector3> rightControleVec = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempSphere.transform.position = new Vector3(Random.Range(-1f, 1f), i, 0);
            leftControlePts.Add(tempSphere);
        }
        for (int j = 0; j < 10; j++)
        {
            var tempSphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempSphere1.transform.position = new Vector3(5 + Random.Range(-1f, 1f), j, 0);
            rightControlePts.Add(tempSphere1);
        }

        foreach(var sphere in leftControlePts)
        {
            leftControleVec.Add(sphere.transform.position);
        }

        foreach (var sphere in rightControlePts)
        {
            rightControleVec.Add(sphere.transform.position);
        }

        var filter = GetComponent<MeshFilter>();
        bottleMesh = new BottleMesh(filter);
        bottleMesh.CreateMesh(leftControleVec, rightControleVec);
        GetComponent<MeshRenderer>().material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            leftControleVec.Clear();
            rightControleVec.Clear();

            foreach (var sphere in leftControlePts)
            {
                leftControleVec.Add(sphere.transform.position);
            }

            foreach (var sphere in rightControlePts)
            {
                rightControleVec.Add(sphere.transform.position);
            }
            bottleMesh.CreateMesh(leftControleVec, rightControleVec);
        }
    }
}
