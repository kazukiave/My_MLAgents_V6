using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;
using Random = System.Random;

public class HunterAgent : Agent
{
    private HuntingArea huntingArea;
    
    #region agent property

    [SerializeField] private float moveSpeedMax = 5f;
    [SerializeField] private float angleMax = 10f;
    #endregion


    private void Start()
    {
        huntingArea = GetComponentInParent<HuntingArea>();
     
        //check public values
        if(moveSpeedMax * 10 > huntingArea.boundRadius)
            Debug.Log("moveSpeedに対して領域が狭すぎるかもよ");
    }

    public override void OnEpisodeBegin()
    {
        transform.position = UnityEngine.Random.insideUnitSphere * huntingArea.boundRadius;
    }


    public override void OnActionReceived(float[] vectorAction)
    {
        float moveSpeed = vectorAction[0];
        float angleRL = vectorAction[1];
        float angleUD = vectorAction[2];
        
        // Set Scale
        moveSpeed *= moveSpeedMax;
        angleRL = Remap(angleRL, 0, 1f, angleMax * -1f, angleMax);
        angleUD =  Remap(angleUD, 0, 1f, angleMax * -1f, angleMax);

        //transform
        transform.position += gameObject.transform.forward * moveSpeed;
        transform.RotateAround(transform.position, Vector3.up, angleRL);
        transform.RotateAround(transform.position, Vector3.right, angleUD);
    }

    public override void Heuristic(float[] actionsOut)
    {
      
    }
    
    public float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void OnCollisionEnter(Collision other)
    {
       var tag =  other.gameObject.tag;
       switch (tag)
       {
           case "hunter":
               break;
           
           case "food":
               break;
       }
    }
}
