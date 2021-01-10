using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainArea : MonoBehaviour
{
    public Vector3 areaMax;
    public Vector3 areaCenter;
    public float sizeX;
    public float sizeY;
    public float sizeZ;


    [NonSerialized]
    public List<Transform> areaWalls;

    [NonSerialized]
    public BoundingBox boundingBox;

    private void Awake()
    {
        areaWalls = new List<Transform>();
        areaWalls.AddRange(gameObject.GetComponentsInChildren<Transform>());
        areaWalls.RemoveAt(0);

        ChangeArea(0);
    }


    private bool firstMake = false;
    private void MakeTrainArea()
    {
        var min = new Vector3(-sizeX / 2f, -sizeY / 2f, -sizeZ / 2f) + areaCenter;
        var max = new Vector3(sizeX / 2f, sizeY / 2f, sizeZ / 2f) + areaCenter;
        boundingBox = new BoundingBox(max, min);
        if(!firstMake)
        {
            areaWalls[0].Rotate(Vector3.forward, 90f);
            areaWalls[1].Rotate(Vector3.forward, -90f);
            areaWalls[2].Rotate(Vector3.forward, 180f);
            areaWalls[4].Rotate(Vector3.right, -90f);
            areaWalls[5].Rotate(Vector3.right, 90f);

            firstMake = true;
        }
        //回転してからScale変える時ワールド座標ではなく、ローカル座標でスケールされるため、上下とかずれる
        //Right
        areaWalls[0].localScale = new Vector3(sizeY * 0.1f, 1f, sizeZ * 0.1f);
        areaWalls[0].position = new Vector3(sizeX / 2.0f,  0, 0) + areaCenter;
        areaWalls[0].name = "Right";

        //Left
        areaWalls[1].localScale = new Vector3(sizeY * 0.1f, 1f, sizeZ * 0.1f);
        areaWalls[1].position = new Vector3(sizeX / -2.0f, 0, 0) + areaCenter;
        areaWalls[1].name = "Left";

        //up
        areaWalls[2].localScale = new Vector3(sizeX * 0.1f, 1f, sizeZ * 0.1f);
        areaWalls[2].position = new Vector3(0, sizeY / 2.0f, 0) + areaCenter;
        areaWalls[2].name = "Up";

        //down
        areaWalls[3].localScale = new Vector3(sizeX * 0.1f, 1f, sizeZ * 0.1f);
        areaWalls[3].position = new Vector3(0, sizeY / -2.0f, 0) + areaCenter;
        areaWalls[3].name = "Down";

        //forward
        areaWalls[4].localScale = new Vector3(sizeX * 0.1f, 1, sizeY * 0.1f);
        areaWalls[4].position = new Vector3(0, 0, sizeZ / 2.0f) + areaCenter;
        areaWalls[4].name = "Forward";

        //back
        areaWalls[5].localScale = new Vector3(sizeX * 0.1f, 1, sizeY * 0.1f);
        areaWalls[5].position = new Vector3(0, 0, sizeZ / -2.0f) + areaCenter;
        areaWalls[5].name = "Back";

       
        foreach (var i in areaWalls)
        {
            i.GetComponent<MeshFilter>().mesh.RecalculateBounds();
            i.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        }
        
    }

    public void ChangeArea(int level)
    {
        switch(level)
        {
            case 0:
                sizeX = 5f;
                sizeY = 2f;
                sizeZ = 5f;
                break;

            case 1:
                sizeX = UnityEngine.Random.Range(3f, 8f);
                sizeY = UnityEngine.Random.Range(3f, 5f);
                sizeZ = UnityEngine.Random.Range(3f, 8f);
                break;

            case 2:
                sizeX = UnityEngine.Random.Range(5f, 8f);
                sizeY = UnityEngine.Random.Range(3f, 5f);
                sizeZ = UnityEngine.Random.Range(5f, 8f);
                break;

            default:
                sizeX = UnityEngine.Random.Range(3f, 15f);
                sizeY = UnityEngine.Random.Range(3f, 5f);
                sizeZ = UnityEngine.Random.Range(3f, 15f);
                break;
        }
        MakeTrainArea();
    }

}
