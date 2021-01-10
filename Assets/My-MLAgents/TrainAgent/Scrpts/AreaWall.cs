using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AreaWall : MonoBehaviour
{
    private List<Transform> areaWalls;
    private Transform parent;
    private TrainArea trainArea;

    

    private void OnCollisionEnter(Collision collision)
    {
        parent = transform.parent;
        trainArea = parent.GetComponent<TrainArea>();
        areaWalls = trainArea.areaWalls;

        int randIdx = Random.Range(0, areaWalls.Count - 1);
        var nextWall = areaWalls[randIdx].GetComponent<MeshFilter>().mesh;
        var nextPos = RandomPtOnMesh(nextWall);
        nextPos += Vector3.Normalize(trainArea.areaCenter - nextPos) * 2f;
        collision.transform.position = nextPos;
        collision.transform.LookAt(trainArea.areaCenter);
    }

    private Vector3 RandomPtOnMesh(Mesh mesh)
    {

        int randIdx = Random.Range(0, mesh.vertices.Length - 1);
        var worldVtx = transform.TransformPoint(mesh.vertices[randIdx]);
        return worldVtx;

        /*
        var max = mesh.bounds.max;
        var min = mesh.bounds.min;
        var cent = mesh.bounds.center;

        float randX = Random.Range(min.x, max.x);
        float randY = Random.Range(min.y, max.y);
        float randZ = Random.Range(min.z, max.z);

        return new Vector3(randX, randY, randZ);
        */
    }
 }
