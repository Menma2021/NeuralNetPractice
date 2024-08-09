using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{
    public int[] layers; // Layers
    public float[][] neurons; // Neurons
    public float[][][] weights; // Weights
    public float fitness; // Fitness

    public NeuralNetwork(int[] layers)
    {
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }
        InitNeurons();
        InitWeights();
    }

    void InitNeurons()
    {
        // Initialize the neurons array with correct layer sizes
        neurons = new float[layers.Length][];
        for (int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new float[layers[i]];
        }
    }

    void InitWeights()
    {
        // Initialize the weights array
        weights = new float[layers.Length - 1][][];
        for (int i = 0; i < layers.Length - 1; i++)
        {
            int neuronsInPreviousLayer = layers[i];
            weights[i] = new float[layers[i + 1]][];

            for (int j = 0; j < layers[i + 1]; j++)
            {
                weights[i][j] = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    weights[i][j][k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
            }
        }
    }

    public float[] FeedForward(float[] inputs)
    {
        // Set input neurons
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        // Feed forward
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < layers[i]; j++)
            {
                float value = 0f;
                for (int k = 0; k < layers[i - 1]; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = (float)Math.Tanh(value);
            }
        }

        // Return output layer
        return neurons[neurons.Length - 1];
    }

    public NeuralNetwork Copy()
    {
        NeuralNetwork copy = new NeuralNetwork(layers);
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    copy.weights[i][j][k] = weights[i][j][k];
                }
            }
        }
        return copy;
    }

    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    if (UnityEngine.Random.Range(0f, 1f) < mutationRate)
                    {
                        weights[i][j][k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                }
            }
        }
    }
}
