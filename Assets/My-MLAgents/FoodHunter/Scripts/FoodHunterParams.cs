using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(GameObject))]
public class FoodHunterParams : MonoBehaviour
{
    [Header("Env Params")]
    [SerializeField] readonly public int maxStep = 1000;
    [SerializeField] private int foodAgentNum = 1;
    [SerializeField] public GameObject foodAgent;

    [SerializeField] private int hunterAgentNum = 1;
    [SerializeField] public GameObject hunterAgent;

    private GameObject boundSphere;
    
    [Header("AgentParam")]
    public float moveSpeedMax = 5f;
    public float angleMax = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        var agents = new List<GameObject>();
        for (int i = 0; i < foodAgentNum; i++)
        {
            var agent = GameObject.Instantiate(foodAgent, transform.position, Quaternion.identity, gameObject.transform );
            agents.Add(agent);
        }
        
        for (int i = 0; i < hunterAgentNum; i++)
        {
            var agent = GameObject.Instantiate(hunterAgent, transform.position, Quaternion.identity,  gameObject.transform);
            agents.Add(agent);
        }

        foreach (var agent in agents)
        {
            agent.GetComponent<FoodHunterAgent>().Init();
        }
        Debug.Log("Child Count=" + gameObject.transform.childCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
