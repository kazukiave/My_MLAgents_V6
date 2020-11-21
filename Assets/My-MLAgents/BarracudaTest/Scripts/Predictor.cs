using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Barracuda;
using TMPro;

public class Predictor : MonoBehaviour
{
    [Header("onnx Model")]
    public NNModel modelAsset;
    private Model m_RuntimeModel;

    [Header("input tensor")]
    public RenderTexture inputTexture;
    public int channelCount = 1; //Gray scale
    public int imageSize = 84;

    [Header("UI")]
    public Text predcitNum;
    public TextMeshPro textMesh;

    [Header("others")]
    private IWorker worker;

    // Start is called before the first frame update
    void Start()
    {
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        foreach (var name in m_RuntimeModel.layers)
        {
            Debug.Log(name);
        }
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, m_RuntimeModel);
    }

    public float[] TexturePredict()
    {
        //Debug.Log("Predicto callsed");

        //IWorker interface, you can execute the model. 
       
        Tensor input = new Tensor(inputTexture, channelCount);
        worker.Execute(input);

        //If the model has a single output, you can use worker.PeekOutput()
        //  Tensor O = m_Worker.PeekOutput("output_layer_name");
        var prediction = worker.PeekOutput("dense_18");
        //Debug.Log(string.Format("prediction length is {0}", prediction.length));

        //Debug.Log(string.Format("prediction is {0}", prediction[0]));
        if (textMesh != null)
        {
            float predict = Mathf.FloorToInt(prediction[0] * 100f) * 0.01f;
            textMesh.text = predict.ToString();
        }
        input.Dispose();
        return prediction.ToReadOnlyArray();
    }
}
