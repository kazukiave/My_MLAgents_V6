using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class FoodHunterAgent : Agent
{
    private HuntingArea huntingArea;
    private FoodHunterParams foodHunterParams;
    
    #region agent property

    private AgentType agentType;
    private float moveSpeedMax;
    private float angleMax ;
    #endregion

    private GameObject brother;
    enum AgentType
    {
        Food = 1,
        Hunter,
    }

    public void Init()
    {
        Debug.Log("Start");
        var parent = gameObject.transform.parent;
        Debug.Log(parent.name);
        huntingArea = parent.GetComponent<HuntingArea>();
        foodHunterParams = parent.GetComponent<FoodHunterParams>();
        MaxStep = foodHunterParams.maxStep;
        
        //set properties
        moveSpeedMax = foodHunterParams.moveSpeedMax;
        angleMax = foodHunterParams.angleMax;
        agentType = transform.CompareTag("food") ? AgentType.Food : AgentType.Hunter;
        
        //Get bady
        brother = GetBrother();

        /*
        //check public values
        if(moveSpeedMax * 10 > huntingArea.boundRadius)
            Debug.Log("moveSpeedに対して領域が狭すぎるかもよ");
        */
    }
    private void Awake()
    {

    }

    private void Start()
    {
       
    }

    public override void OnEpisodeBegin()
    {
        transform.position = huntingArea.GetInitPos();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((int)agentType);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float moveSpeed = vectorAction[0];
        float angleRL = vectorAction[1];
      //  float angleUD = vectorAction[2];
        
        // Set Scale
        //Vector Action range is -1 to 1
        moveSpeed = Remap(moveSpeed, -1f, 1f, 0f, moveSpeedMax);
        angleRL = Remap(angleRL, -1f, 1f, angleMax * -1f, angleMax);
      //  angleUD =  Remap(angleUD, -1f, 1f, angleMax * -1f, angleMax);

        //transform
        transform.RotateAround(transform.position, Vector3.up, angleRL);
        //transform.RotateAround(transform.position, Vector3.right, angleUD);
        
        var nextPos = transform.position + gameObject.transform.forward * moveSpeed;
        var distToCenter = Vector3.Distance(huntingArea.boundCenter, nextPos);
        
        //エリア外にぬけたとき
        if (distToCenter > huntingArea.boundRadius - 1f)
        {
            AddReward(-1f);
        }
        else
        {
            transform.position = nextPos;
        }
        
        //dostance to brother
        float distBrother = Vector3.Distance(brother.transform.position, transform.position);
        var radius = huntingArea.boundRadius * 2f;
        if (agentType == AgentType.Food)
        {
            AddReward((distBrother / radius));
        }
        else
        {
            AddReward((distBrother / radius) * -1f);
        }
        
    }

    public override void Heuristic(float[] actionsOut)
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        for (int i = 0; i < 2; i++)
        {
            actionsOut[i] = Random.Range(-1f, 1f);
        }
    }
    
    public float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void OnCollisionEnter(Collision other)
    {
        CollisionEvent(other);
    }

    private void OnCollisionStay(Collision other)
    {
       // CollisionEvent(other);
    }

    private void CollisionEvent(Collision other)
    {
        var hitTag =  other.gameObject.tag;
        if(hitTag == "boundry") return;
        
        if (agentType == AgentType.Food)
        {
            agentType = AgentType.Hunter;
        }
        else
        {
            agentType = AgentType.Food;
        }
        
        /*
        if (agentType == AgentType.Food)
        {
            switch (hitTag)
            {
                case "hunter":
                    AddReward(-0.5f);
                    break;

                case "food":
                    break;
            }
        }
       
        if (agentType == AgentType.Hunter)
        {
            switch (hitTag)
            {
                case "hunter":
                    break;

                case "food":
                    AddReward(0.5f);
                    break;
            }
        }
        */
    }

    private GameObject GetBrother()
    {
        GameObject rtnVal = null;
        var parent = gameObject.transform.parent;
  
        Debug.Log("childCOunt" + parent.childCount);
        
       for(int i = 0; i < parent.childCount; i++)
       {
           var child = parent.GetChild(i);
            if (agentType == AgentType.Food)
            {
                if (child.gameObject.CompareTag("hunter"))
                {
                    rtnVal = child.gameObject;
                }
            }
            else
            {
                if (child.gameObject.CompareTag("food"))
                {
                    rtnVal = child.gameObject;
                }
            }
        }
        if(rtnVal == null)
            Debug.Log("failed");
        return rtnVal;
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
