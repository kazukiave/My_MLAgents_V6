
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    [SerializeField] TrainArea trainArea = null;
    [SerializeField] GameObject foodPrefab = null;
    private float timeleft = 0;


    //1分　BOXを降らせる
    private void Update()
    {
        timeleft -= Time.deltaTime;
        if (timeleft <= 0.0)
        {
            timeleft = 1.0f;

            for (int j = 0; j < Random.Range(1, 5); j++)
            {
                var tempCube = Instantiate(foodPrefab, RandomFoodPos(), Quaternion.identity);
                Destroy(tempCube, 7f);
            }
        }
    }

    private Vector3 RandomFoodPos()
    { 
        var max = trainArea.boundingBox.Max ;
        var min = trainArea.boundingBox.Min ;

        var randX = Random.Range(min.x, max.x);
        var randY = Random.Range(min.y + 1.5f, max.y - 0.5f);

        var randZ = Random.Range(min.z, max.z);

        return new Vector3(randX, randY, randZ);
        /*
        var a = trainArea.areaMax;
        var topMesh = GameObject.Find("Up").GetComponent<MeshFilter>().mesh;
        
        int randIdx = Random.Range(0, topMesh.vertices.Length - 1);
        var randTopVtx = transform.TransformPoint(topMesh.vertices[randIdx]);
        return randTopVtx;
        */
    }
}
