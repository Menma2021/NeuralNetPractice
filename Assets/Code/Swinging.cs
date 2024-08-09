using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinging : MonoBehaviour
{
    public Rigidbody characterRigidbody;  // The Rigidbody of the character
    public GeneticAlgorithm ga;  // Reference to the GeneticAlgorithm component
    public int individualIndex = 0;  // Index of the individual in the population

    public NeuralNetwork neuralNetwork;  // Neural network used for control
    private FixedJoint fixedJoint;  // Fixed joint component connecting the character to the swing
    private float legForce = 220f;  // Force applied to the character
    private bool isSwinging = true;  // Flag to determine if the character is swinging
    public bool hasJumped = false;  // Flag to determine if the character has jumped
    private float swingMomentumAtJump = 0f;  // Store the momentum just before jumping

    private float decisionInterval = 0.3f;  // Time interval between decision updates
    private float decisionTimer;  // Timer to track time for decision updates
    private float[] lastOutputs = new float[3];  // Store the last neural network outputs
    private float[] currentOutputs = new float[3];  // Store the current neural network outputs

    public Transform ropeTransform; // Transform of the rope

    void Start()
    {
        ga = FindObjectOfType<GeneticAlgorithm>();

        neuralNetwork = ga.population[individualIndex];
        characterRigidbody = GetComponent<Rigidbody>();

        fixedJoint = GetComponent<FixedJoint>();

        decisionTimer = decisionInterval;  // Initialize the decision timer
    }

    void Update()
    {

        decisionTimer -= Time.deltaTime;  // Decrement the timer

        if (decisionTimer <= 0)
        {
            // Update decision outputs
            float[] inputs = GetInputs();
            // Update lastOutputs with the current decision
            lastOutputs = (float[])currentOutputs.Clone();
            currentOutputs = neuralNetwork.FeedForward(inputs); // Update current decision
            decisionTimer = decisionInterval;  // Reset the timer
        }

        if (isSwinging)
        {
            // Apply swing force based on neural network outputs
            float direction = currentOutputs[0] - currentOutputs[1];
            ApplySwingForce(direction);

            // Check if the neural network decides to jump
            if (currentOutputs[2] > 0.5f && !hasJumped)
            {
                swingMomentumAtJump = characterRigidbody.velocity.magnitude;  // Record momentum before jumping
                JumpOffSwing();
                hasJumped = true;  // Set flag to avoid multiple jumps
            }
        }
    }

    public float[] GetInputs()
    {
        // Compute swing angle and angular velocity
        float swingAngle = ropeTransform.localEulerAngles.z;
        swingAngle = Mathf.DeltaAngle(0, swingAngle) / 180f + 0.02f; // Normalizing angle
        float swingAngularVelocity = characterRigidbody.velocity.x;
        return new float[] { swingAngle, swingAngularVelocity };
    }

    void ApplySwingForce(float direction)
    {
        // Apply force to the Rigidbody in the calculated swing direction
        Vector3 swingDirection = characterRigidbody.transform.right * direction;
        characterRigidbody.AddForce(swingDirection.normalized * legForce * Time.deltaTime, ForceMode.Force);
    }

    void JumpOffSwing()
    {
        isSwinging = false;

        if (fixedJoint != null)
        {
            // Destroy the FixedJoint to detach the character from the swing
            Destroy(fixedJoint);
        }

        // Apply momentum based on current swing velocity
        Vector3 momentum = characterRigidbody.velocity;
        characterRigidbody.isKinematic = false;
        characterRigidbody.AddForce(momentum, ForceMode.VelocityChange);
    }

    public float GetFinalPositionX()
    {
        // Apply penalty if x-velocity is negative
        float xVelocity = characterRigidbody.velocity.x;
        //float penalty = xVelocity < -0.02 ? -2f : 0f; // Penalize if moving backward
        float xPosition = characterRigidbody.position.x;
        float timingPenalty = !hasJumped ? -5f : 0f;  // Penalize for not jumping
        return xPosition + timingPenalty;  // + penalty
    }

    public float GetFinalMomentum()
    {
        return swingMomentumAtJump; // Return the recorded swing momentum
    }

    public NeuralNetwork GetNeuralNetwork()
    {
        return neuralNetwork;
    }

    public float[] GetLastOutputs()
    {
        return lastOutputs;
    }

    public float[] GetCurrentOutputs()
    {
        return currentOutputs;
    }
}
