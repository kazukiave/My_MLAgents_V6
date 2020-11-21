using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Barracuda;



public class TestRun : MonoBehaviour
{
    /// <summary>
    /// https://docs.unity3d.com/Packages/com.unity.barracuda@1.1/manual/Worker.html
    /// https://towardsdatascience.com/how-to-build-your-tensorflow-keras-model-into-an-augmented-reality-app-18405c36acf5
    /// </summary>


    [Header("onnx Model")]
    public NNModel modelAsset;
    private Model m_RuntimeModel;

    [Header("input tensor")]
    public RenderTexture inputTexture;
    public int channelCount = 1; //Gray scale
    public int imageSize = 84;

    [Header("UI")]
    public Text predcitNum;

    // Start is called before the first frame update
    void Start()
    {
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        foreach(var name in m_RuntimeModel.layers)
        {
            Debug.Log(name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TexturePredict(inputTexture);
        }
    }

    private void TexturePredict(RenderTexture inputTexture)
    {
        Debug.Log("Predicto callsed");

        //IWorker interface, you can execute the model. 
        var worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, m_RuntimeModel);

        Tensor input = new Tensor(inputTexture, channelCount);
        worker.Execute(input);

        //If the model has a single output, you can use worker.PeekOutput()
        //  Tensor O = m_Worker.PeekOutput("output_layer_name");
        var prediction = worker.PeekOutput("dense_18");
        Debug.Log(string.Format("prediction length is {0}", prediction.length));

        Debug.Log(string.Format("prediction is {0}", prediction[0]));
        predcitNum.text = prediction[0].ToString();
        input.Dispose();
    }

    private void TensorPredict(Tensor input)
    {
        var worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, m_RuntimeModel);
        worker.Execute(input);

        //If the model has a single output, you can use worker.PeekOutput()
        //Tensor O = m_Worker.PeekOutput("output_layer_name");
        var prediction = worker.PeekOutput("Sigmoid");

        input.Dispose();
    }
}
