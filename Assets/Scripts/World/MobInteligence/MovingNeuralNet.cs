using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingNeuralNet : MonoBehaviour
{
    private static MovingNeuralNet instance;
    public static MovingNeuralNet Instance
    {
        get
        {
            return instance;
        }
    }

    public static NeuralNetwork.NeuralNet thisNet;

    private static List<NeuralNetwork.DataSet> DataSets = new List<NeuralNetwork.DataSet>();

    private void Awake()
    {
        instance = this;
        thisNet = new NeuralNetwork.NeuralNet(4, 3, 1);

        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 1.0, 1.0, 0.5 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 0.0, 1.0, 0.5 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 1.0, 0.0, 0.5 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 0.0, 0.0, 0.5 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 1.0, 1.0, 0.5 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 0.0, 1.0, 0.5 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 1.0, 0.0, 0.5 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 0.0, 0.0, 0.5 }, new double[] { 0.5 }));

        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 1.0, 1.0, 0.1 }, new double[] { 0.1 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 1.0, 0.0, 0.1 }, new double[] { 0.1 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 0.5, 0.0, 0.1 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 0.5, 1.0, 0.1 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 1.0, 1.0, 0.1 }, new double[] { 0.1 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 1.0, 0.0, 0.1 }, new double[] { 0.1 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 0.5, 0.0, 0.1 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 0.5, 1.0, 0.1 }, new double[] { 0.5 }));

        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 1.0, 1.0, 0.9 }, new double[] { 0.9 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 0.0, 1.0, 0.9 }, new double[] { 0.9 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 1.0, 0.5, 0.9 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 1.0, 0.0, 0.5, 0.9 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 1.0, 1.0, 0.9 }, new double[] { 0.9 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 0.0, 1.0, 0.9 }, new double[] { 0.9 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 1.0, 0.5, 0.9 }, new double[] { 0.5 }));
        DataSets.Add(new NeuralNetwork.DataSet(new double[] { 0.0, 0.0, 0.5, 0.9 }, new double[] { 0.5 }));

        thisNet.Train(DataSets,10);
    }
}
