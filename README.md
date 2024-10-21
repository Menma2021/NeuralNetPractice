# Neural Network Test project

## Overview
This Unity project was created as a way to learn how simple genetic algorithm works. Projects purpose is to show how a neural network can learn how to swing and jump from it using a genetic algorithm 

## Features
1. Genetic Algorithm: A genetic algorithm is used to evolve the neural networks over multiple generations. Individuals with the best performance are selected for reproduction, creating the next generation
2. Neural Network: Each character is controlled by a neural network, which processes inputs like swing angle and velocity and outputs decisions such as applying forward or backwards force or jumping off the swing
3. Swinging Mechanics: The simulation mimics swinging physics, allowing the characters to control their movements and decide when to jump off
4. Real-time Statistics: The UI displays real-time statistics for the random current offspring, including swing angle, velocity, neural network inputs/outputs, and decisions

## Project Structure
### Scripts Overview
1. GeneticAlgorithm.cs:
Manages the genetic algorithm, initializing the population of neural networks, evolving them through elitism, crossover, and mutation, and controlling the flow of generations
2. NeuralNetwork.cs:
Defines a simple feedforward neural network with multiple layers, capable of receiving inputs (swing angle, velocity) and producing outputs (swing direction, jump decision). Supports copying and mutation for genetic evolution
3. SimulationController.cs:
Handles the creation and destruction of characters at the start and end of each generation. It assigns neural networks to characters and calculates fitness based on final position and momentum
4. Swinging.cs:
The core logic for swinging mechanics, where the neural network's decisions (swinging force and jump) are applied to the characters.
5. UIManager.cs:
Displays real-time statistics and decisions made by the neural network for the currently selected character. The UI shows swing angle, velocity, position, and decisions regarding swinging and jumping.

## How It Works
### Initialization
At the start of the simulation, a population of neural networks is created. Each neural network is assigned to a character, which is then placed on the swing. The characters swing for a fixed duration, with each one controlling its swinging motion based on the neural network's outputs

### Fitness Evaluation
At the end of each generation, the fitness is calculated based on the distance traveled and the momentum at the point of jumping off the swing. Fitness is a combination of the character's final position and the momentum before the jump

### Evolution
The top-performing neural networks (based on fitness) are selected to form the next generation. Through crossover and mutation, new neural networks are created from the best performers to replace the less fit individuals. This process continues across generations, refining the population over time

## Neural Network Decisions
The neural network receives two inputs:

1. Swing angle (normalized)
2. Swing angular velocity (x-axis)

It produces three outputs:

1. Swinging force direction (based on subtracting two outputs)
2. Jump decision (if the third output exceeds a threshold)

## How to Use
1. Setup Unity
Open the project in Unity 2019 or higher (personally used 2019, not sure how it will work on future versions). Ensure that the physics settings in Unity are configured properly for the Rigidbody-based swinging mechanics
2. Start the simulation
3. Monitor progress with UI
4. Tweak parameters if you want to, such as:
-Population size: Number of neural networks/characters in each generation
-Mutation Rate: Probability of mutation during reproduction
-Generation Duration: Time for each generation to run

## Contribution
Contributions are welcome! If you have suggestions or improvements, feel free to fork the repository and submit a pull request

## License
This project is licensed under the MIT License
