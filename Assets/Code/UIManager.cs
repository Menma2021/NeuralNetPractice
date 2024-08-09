using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text statisticsText;  // Reference to the Text component for displaying statistics
    public float displayInterval = 5f; // Time interval to switch to a new random offspring

    private SimulationController simulationController;
    private float timer = 0f;
    private GameObject currentCharacter;

    void Start()
    {
        // Find the SimulationController in the scene
        simulationController = FindObjectOfType<SimulationController>();
        if (simulationController == null)
        {
            Debug.LogError("SimulationController not found in the scene.");
        }

        // Initialize the timer and set the first character
        timer = displayInterval;
        SetRandomCharacter();
    }

    void Update()
    {
        if (statisticsText != null && simulationController != null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SetRandomCharacter();
                timer = displayInterval; // Reset the timer
            }

            // Update statistics continuously
            if (currentCharacter != null)
            {
                UpdateStatistics();
            }
        }
    }

    void SetRandomCharacter()
    {
        if (simulationController.characters.Count > 0)
        {
            // Pick a random character from the list
            int randomIndex = Random.Range(0, simulationController.characters.Count);
            currentCharacter = simulationController.characters[randomIndex];
        }
    }

    void UpdateStatistics()
    {
        if (currentCharacter != null)
        {
            // Get the Swinging component from the current character
            Swinging swinging = currentCharacter.GetComponentInChildren<Swinging>();
            if (swinging != null)
            {
                // Gather information
                float finalPositionX = swinging.GetFinalPositionX();
                float finalMomentum = swinging.GetFinalMomentum();
                Transform capsuleTransform = currentCharacter.transform.Find("Capsule");
                float xVelocity = capsuleTransform.GetComponent<Rigidbody>().velocity.x;

                // Get neural network outputs and inputs
                float[] inputs = swinging.GetInputs();
                float[] currentOutputs = swinging.GetCurrentOutputs();
                float[] lastOutputs = swinging.GetLastOutputs();

                // Format output decisions
                string currentDecisions = $"Swing Direction: {(currentOutputs[0] - currentOutputs[1]):F2}, Jump Decision: {(currentOutputs[2]):F2}";
                string lastDecisions = $"Swing Direction: {(lastOutputs[0] - lastOutputs[1]):F2}, Jump Decision: {(lastOutputs[2]):F2}";

                // Format inputs
                string inputsText = $"Swing Angle: {inputs[0]:F2}, Angular Velocity: {inputs[1]:F2}";

                // Update the UI text with statistics, decisions, and inputs
                statisticsText.text = $"Current Offspring Statistics:\n" +
                                      $"Final X Position: {finalPositionX:F2}\n" +
                                      $"Swing Momentum: {finalMomentum:F2}\n" +
                                      $"X Velocity: {xVelocity:F2}\n" +
                                      $"Inputs:\n{inputsText}\n" +
                                      $"Current Decisions:\n{currentDecisions}\n" +
                                      $"Last Decisions:\n{lastDecisions}";
            }
        }
        else
        {
            statisticsText.text = "No characters available.";
        }
    }
}