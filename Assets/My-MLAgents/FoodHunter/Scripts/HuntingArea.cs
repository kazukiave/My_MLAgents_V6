using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class HuntingArea : MonoBehaviour
{

    
    #region Bound Property

    private GameObject boundSphere;
    public Vector3 boundCenter = Vector3.zero;
    
    [Range(1, 50)] 
    public int boundRadius = 1;
    #endregion

    [SerializeField] private GameObject zWall = null;
    [SerializeField] private GameObject xWall = null;
    [SerializeField] private GameObject plane = null;
    
    // Start is called before the first frame update
    void Start()
    {
        boundCenter = transform.position;
        
        zWall.transform.localScale = new Vector3( boundRadius * 2f, 5f, 1f);
        xWall.transform.localScale = new Vector3( 1f, 5f, boundRadius * 2f);
   
        zWall.transform.position = new Vector3( 0, 0, boundRadius);
        xWall.transform.position = new Vector3(boundRadius, 0, 0);
        
        var postZ = GameObject.Instantiate(zWall);
        var postX = GameObject.Instantiate(xWall);
        postZ.transform.position = new Vector3( 0, 0, boundRadius * -1f);
        postX.transform.position = new Vector3( boundRadius * -1f, 0, 0);
        
        plane.transform.localScale = new Vector3(boundRadius *0.2f, boundRadius *0.2f, boundRadius *0.2f);
        /*
        boundSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        boundSphere.transform.position = boundCenter;
        boundSphere.tag = "boundry";
        boundSphere.transform.localScale = Vector3.one * boundRadius * 2f;
        boundSphere.GetComponent<MeshRenderer>().enabled = false; 
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetInitPos()
    {
        var randPos = boundCenter + (UnityEngine.Random.insideUnitSphere * boundRadius);
        randPos.y = 1f;
        return randPos;
    }

    private void OnDrawGizmosSelected()
    {
        // Display the explosion boundRadius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(boundCenter, boundRadius);
    }
}
