using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;


public class TrainAgent : Agent
{
    public Transform pibot;

    private int max_steps = 150000;

    private Transform initialTrs;
    private TrainAgentSetting trainAgentSetting;
    private Rigidbody agentRd;
    private float swarmDist =1f;
    private Material agentMat;
    private Color AgentCol;

    public void Awake()
    {
        initialTrs = gameObject.transform;
        agentMat = GetComponent<MeshRenderer>().material;
        agentRd = GetComponent<Rigidbody>();
        trainAgentSetting = transform.parent.GetComponent<TrainAgentSetting>();
    }

    public override void Initialize()
    {
        gameObject.transform.position = initialTrs.position;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        int rewadType = (int)trainAgentSetting.rewadType;
        sensor.AddOneHotObservation(rewadType, 1);
    }
 
    public void MoveAgent(float[] act)
    {
        var rotX = transform.localEulerAngles.x;
        var rotY = transform.localEulerAngles.y;
        var rotZ = transform.localEulerAngles.z;

        rotX = rotX >= 180f ? rotX - 360f : rotX;
        rotY = rotY >= 180f ? rotY - 360f : rotY;
        rotZ = rotZ >= 180f ? rotZ - 360f : rotZ;


        //Objectが逆さまにならないように
        if (Mathf.Abs(rotZ) > 3f)
        {
            var angle = Mathf.Abs(rotZ) < 10 ? 1f : 10f;
            angle = rotZ > 0 ? angle * -1f : angle;
            rotZ += angle;
        }
        else
        {
            rotZ = 0;
        }


        //vectorAction range is -1to1

        //act[2] = (act[2] + 1f) * 0.5f;
        //gameObject.transform.position += transform.forward * 0.1f;
        if(Math.Abs(rotX + act[1] * 2f) >= 60)
        {
            act[1] = 0;
        }
        agentRd.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(rotX + act[1] * 2f, rotY  + act[0] * 4f, rotZ);
        agentRd.AddForce(transform.forward * 0.5f, ForceMode.VelocityChange);
        /* transform.Rotate(pibot.up, act[0] * 4f);
        transform.Rotate(pibot.right, act[1] * 2f);
       
     
        */
    }

    public void CalcuRewad()
    {
        /*
        int count = 0;
        var agents = trainAgentSetting.agents;
        for (int i = 0; i < agents.Count; i++)
        {
            float dist = Vector3.Distance(agents[i].transform.position, transform.position);
            if (dist < swarmDist)
                count++;
        }
        float reward = (float)count / (float)agents.Count;
        AddReward(reward * 0.00001f);
      
        agentMat.color = new Color(reward, 0, 0);
        return;
          */
        /*
        switch (trainAgentSetting.rewadType)
        {
            //Swarms
            case TrainAgentSetting.RewadType.boids:

                break;
        }
        */
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        MoveAgent(vectorAction);
        CalcuRewad();
    }

    
    public override void Heuristic(float[] actionsOut)
    {
      
        for (int i = 0; i < actionsOut.Length; i++)
        {
            //actionsOut[i] = Random.Range(-1f, 1f);
            actionsOut[i] = 0f;
        }

        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
        MoveAgent(actionsOut);
    }

    public override void OnEpisodeBegin()
    {

        gameObject.transform.position = Vector3.zero;

        if (gameObject.name == "0")
        {
            var totalStep = (float)Academy.Instance.TotalStepCount;
            var stepRatio = totalStep / (float)max_steps;
            if (0 < stepRatio && stepRatio < 0.25f)
            {
                GameObject.Find("TrainArea").GetComponent<TrainArea>().ChangeArea(0);
            }
            else if (0.25f < stepRatio && stepRatio < 0.50f)
            {
                GameObject.Find("TrainArea").GetComponent<TrainArea>().ChangeArea(1);
            }
            else if (0.50f < stepRatio && stepRatio < 0.75f)
            {
                GameObject.Find("TrainArea").GetComponent<TrainArea>().ChangeArea(2);
            }
            else if (0.75f < stepRatio)
            {
                GameObject.Find("TrainArea").GetComponent<TrainArea>().ChangeArea(3);
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Food") && trainAgentSetting.rewadType == TrainAgentSetting.RewadType.feed)
        {
            AddReward(0.1f);
            collision.transform.gameObject.SetActive(false);
        }

        if(collision.transform.CompareTag("Wall"))
        {
            AddReward(-0.1f);
        }
    }

    public void OnDrawGizmos()
    {
        var col = Color.yellow;
        col.a = 0.1f;
        Gizmos.color = col;
        
        Gizmos.DrawSphere(transform.position, swarmDist);

        Gizmos.DrawLine(transform.position, transform.forward * 100f);
    }
}
