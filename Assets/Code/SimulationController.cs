using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    public GeneticAlgorithm ga;
    public GameObject characterPrefab;
    public Transform spawnPoint;

    public List<GameObject> characters;
    private float elapsedTime = 0f;

    void Start()
    {
        StartGeneration();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= ga.generationDuration)
        {
            EndGeneration();
            StartGeneration();
            elapsedTime = 0f;
        }
    }

    void StartGeneration()
    {
        characters = new List<GameObject>();
        for (int i = 0; i < ga.populationSize; i++)
        {
            // Instantiate the character prefab
            GameObject character = Instantiate(characterPrefab, spawnPoint.position + new Vector3(0, 0, i * 6.47f), spawnPoint.rotation);

            // Find the Capsule child in the instantiated prefab
            Transform capsuleTransform = character.transform.Find("Capsule");

            // Get the Swinging component from the Capsule
            Swinging swinging = capsuleTransform.GetComponent<Swinging>();

            // Assign values to the Swinging component
            swinging.ga = ga;
            swinging.individualIndex = i;

            // Add character to the list
            characters.Add(character);
        }
    }

    void EndGeneration()
    {
        // Fitness calculation is handled before destroying the characters
        foreach (GameObject character in characters)
        {
            Swinging swinging = character.GetComponentInChildren<Swinging>();
            if (swinging != null)
            {
                // Combine final position and momentum into fitness
                float finalPositionX = swinging.GetFinalPositionX();
                float finalMomentum = swinging.GetFinalMomentum();
                ga.population[swinging.individualIndex].fitness = finalPositionX + finalMomentum * 0.5f; // Adjust the weight as needed
            }
            Destroy(character);
        }
    }

}
