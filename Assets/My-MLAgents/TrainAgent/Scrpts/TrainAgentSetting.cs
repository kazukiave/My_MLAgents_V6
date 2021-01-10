using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAgentSetting : MonoBehaviour
{
    
    public GameObject agentPrefab;
    public int agentNum;

    [NonSerialized]
    public List<GameObject> agents;

    [NonSerialized]
    public RewadType rewadType;

    [NonSerialized]
    public FoodController foodController;

    public enum RewadType
    {
        boids = 0,
        feed = 1,
    }

    // Start is called before the first frame update
    void Start()
    {
        foodController = GetComponent<FoodController>();

        /*
            IEnumerableなメソッドはそのまま呼び出せないからです。
            StartCoroutine(pupupu.muteki());
            とする必要があります。
         */

        rewadType = RewadType.feed;
        //foodController.StartRainCoroutine();

        agents = new List<GameObject>();
        for (int i = 0; i < agentNum; i ++)
        {
            var tempAgent = Instantiate(agentPrefab, transform);
            agents.Add(tempAgent);
            tempAgent.name = i.ToString();
        }

    }

}
