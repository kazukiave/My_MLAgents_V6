using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPramTest : MonoBehaviour
{
    [SerializeField][Range(1f, 10f)] float rightLeft = 1f;
    [SerializeField][Range(1f, 10f)] float upDown = 1f;
    [SerializeField][Range(0.1f, 1f)] float forward = 0.1f;

    private Rigidbody agentRd;
    private Transform trs;

    int stepCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        agentRd = GetComponent<Rigidbody>();
        trs = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        agentRd.AddForce(transform.forward * forward, ForceMode.VelocityChange);

        stepCount++;
        if (stepCount < 20) return;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(transform.up, 1f * rightLeft);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(transform.up, -1f * rightLeft);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(transform.right, 1f * upDown);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(transform.right, -1f * upDown);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = trs.rotation;
        }

        stepCount = 0;
    }
}
