using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.Barracuda;


public class BottleAgent : Agent
{
    BottleMesh bottleMesh;
    Predictor predictor;

    [Header("RightPos")]
    private float rightPosX;
    private List<Vector3> rightCtlPos = new List<Vector3>();
    private float rightPosMin = 0;
    private float rightPosMax;


    [Header("LeftPos")]
    private float leftPosX;
    private List<Vector3> leftCtlPos = new List<Vector3>();
    private float leftPosMin;
    private float leftPosMax = 0;

    [Header("BottleProperty")]
    public int controllPtsCount = 10;
    public bool isMirror = false;

    [Header("Others")]
    public GameObject heightMax;
    public GameObject heightMin;
    public SpriteRenderer targetSprit;
    public List<RenderTexture> renderTextures;


    private void Start()
    {
        predictor = GetComponent<Predictor>();
        InitializeBottle();
        bottleMesh = new BottleMesh(GetComponent<MeshFilter>());
        bottleMesh.CreateMesh(leftCtlPos, rightCtlPos);

        int agentNum = int.Parse(gameObject.name.Split('(')[1].Split(')')[0]);
        Debug.Log(agentNum);
        predictor.inputTexture = renderTextures[agentNum];
        GetComponentInChildren<Camera>().targetTexture = renderTextures[agentNum];
        
    }

    public override void OnEpisodeBegin()
    {
      
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        for(int i = 0; i < rightCtlPos.Count; i++)
        {
            var tempX = rightCtlPos[i].x + vectorAction[i];
            tempX = Mathf.Clamp(tempX, rightPosMin, rightPosMax);
            rightCtlPos[i] = new Vector3(tempX , rightCtlPos[i].y, 0);
        }

        if (!isMirror)
        {
            for (int i = 0; i < leftCtlPos.Count; i++)
            {
                int idx = i + rightCtlPos.Count;
                var tempX = leftCtlPos[i].x + vectorAction[idx];
                tempX = Mathf.Clamp(tempX, leftPosMin, leftPosMax);
                leftCtlPos[i] = new Vector3(tempX, leftCtlPos[i].y, 0);
            }
        }
        else
        {
            for (int i = 0; i < leftCtlPos.Count; i++)
            {
                int idx = i + rightCtlPos.Count;
                var tempX = rightCtlPos[i].x * -1f;
                tempX = Mathf.Clamp(tempX, leftPosMin, leftPosMax);
                leftCtlPos[i] = new Vector3(tempX, leftCtlPos[i].y, 0);
            }
        }

        bottleMesh.CreateMesh(leftCtlPos, rightCtlPos);
        var predict = predictor.TexturePredict();
        SetReward(predict[0]);
    }

    public override void Heuristic(float[] actionsOut)
    {
        int iteratCount = isMirror ? rightCtlPos.Count : rightCtlPos.Count * 2;
        for(int i = 0; i < iteratCount; i++)
        {
            actionsOut[i] = Random.Range(-1f, 1f);
        }
    }


    private void InitializeBottle()
    {
        rightCtlPos.Clear();
        leftCtlPos.Clear();

        float bottleHeight = (heightMax.transform.position.y - heightMin.transform.position.y);
        var bounds = targetSprit.bounds;
        float spritWidth = bounds.max.x - bounds.min.x;
        float quadWidth = spritWidth / 4f;

        rightPosX = quadWidth;
        leftPosX = -quadWidth;

        rightPosMax = bounds.max.x;
        leftPosMin = bounds.min.x;


        float difference = (float)bottleHeight / (float)controllPtsCount;
      
        for (int i = 0; i < controllPtsCount; i++)
        {
            float rightX = Random.Range(-quadWidth, quadWidth) + rightPosX ;
            float leftX = Random.Range(-quadWidth, quadWidth) + leftPosX;

            float y = heightMin.transform.position.y + (difference * i);
            rightCtlPos.Add(new Vector3(rightX, y, 0));
            leftCtlPos.Add(new Vector3(leftX, y, 0));
        }
       // Debug.Log(rightCtlPos.Count + " " + leftCtlPos.Count);
    }
}
