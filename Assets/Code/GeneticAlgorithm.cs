using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public int populationSize = 250;
    public int[] layers = new int[] { 2, 30, 3 }; // Input, hidden, output layers
    public List<NeuralNetwork> population;
    public float mutationRate = 0.08f;
    public int generation = 1;
    public float generationDuration = 45f; // 45 seconds for each generation

    private float elapsedTime = 0f;

    void Start()
    {
        InitPopulation();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= generationDuration)
        {
            EvolvePopulation();
            elapsedTime = 0f;
        }
    }

    void InitPopulation()
    {
        population = new List<NeuralNetwork>();
        for (int i = 0; i < populationSize; i++)
        {
            population.Add(new NeuralNetwork(layers));
        }
    }

    void EvolvePopulation()
    {
        List<NeuralNetwork> newPopulation = new List<NeuralNetwork>();

        // Sort by fitness
        population.Sort((a, b) => b.fitness.CompareTo(a.fitness));

        // Elitism: Keep the top 15 individuals
        for (int i = 0; i < 15; i++)
        {
            newPopulation.Add(population[i].Copy());
        }

        // Generate new individuals through crossover and mutation
        while (newPopulation.Count < populationSize)
        {
            NeuralNetwork parent1 = population[UnityEngine.Random.Range(0, 15)];
            NeuralNetwork parent2 = population[UnityEngine.Random.Range(0, 15)];

            NeuralNetwork child = Crossover(parent1, parent2);
            child.Mutate(mutationRate);
            newPopulation.Add(child);
        }

        // Replace old population with new population
        population = newPopulation;
        generation++;
    }

    NeuralNetwork Crossover(NeuralNetwork parent1, NeuralNetwork parent2)
    {
        NeuralNetwork child = new NeuralNetwork(layers);
        for (int i = 0; i < parent1.weights.Length; i++)
        {
            for (int j = 0; j < parent1.weights[i].Length; j++)
            {
                for (int k = 0; k < parent1.weights[i][j].Length; k++)
                {
                    child.weights[i][j][k] = UnityEngine.Random.value > 0.5f ? parent1.weights[i][j][k] : parent2.weights[i][j][k];
                }
            }
        }
        return child;
    }
}
