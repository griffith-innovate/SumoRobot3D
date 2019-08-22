#region Includes
using System;
using System.Collections.Generic;
#endregion

/*
================================================================================
Agent combines a genotype and a feedforward neural network (FNN).

Original code by Samuel Arzt (March 2017) at:
https://github.com/ArztSamuel/Applying_EANNs
================================================================================
 */
public class EANN_Agent : IComparable<EANN_Agent> {
    #region Members
    // The underlying genotype of this agent
    public Genotype Genotype { get; private set; }


    // The feedforward neural network which was constructed from the genotype of this agent.
    public NeuralNetwork FNN { get; private set; }

    private bool isAlive = false;

    // Whether this agent is currently alive (actively participating in the simulation).
    public bool IsAlive {
        get { return isAlive; }
        private set {
            if (isAlive != value) {
                isAlive = value;

                if (!isAlive && AgentDied != null)
                    AgentDied(this);
            }
        }
    }
    // Event for when the agent died (stopped participating in the simulation).
    public event Action<EANN_Agent> AgentDied;
    #endregion

    #region Constructors
    // Initialises a new agent from given genotype, constructing a new feedfoward neural network from
    // the parameters of the genotype.

    public EANN_Agent(Genotype genotype, NeuralLayer.ActivationFunction defaultActivation, params uint[] topology) {
        IsAlive = false;
        this.Genotype = genotype;
        FNN = new NeuralNetwork(topology);
        foreach (NeuralLayer layer in FNN.Layers)
            layer.NeuronActivationFunction = defaultActivation;

        //Check if topology is valid
        if (FNN.WeightCount != genotype.ParameterCount)
            throw new ArgumentException("The given genotype's parameter count must match the neural network topology's weight count.");

        //Construct FNN from genotype
        IEnumerator<float> parameters = genotype.GetEnumerator();
        foreach (NeuralLayer layer in FNN.Layers) //Loop over all layers
        {
            for (int i = 0; i < layer.Weights.GetLength(0); i++) //Loop over all nodes of current layer
            {
                for (int j = 0; j < layer.Weights.GetLength(1); j++) //Loop over all nodes of next layer
                {
                    layer.Weights[i, j] = parameters.Current;
                    parameters.MoveNext();
                }
            }
        }
    }
    #endregion

    #region Methods
    // Resets this agent to be alive again.
    public void Reset() {
        Genotype.Evaluation = 0;
        Genotype.Fitness = 0;
        IsAlive = true;
    }

    // Kills this agent (sets IsAlive to false).
    public void Kill() {
        IsAlive = false;
    }

    #region IComparable
    // Compares this agent to another agent, by comparing their underlying genotypes.
    public int CompareTo(EANN_Agent other) {
        return this.Genotype.CompareTo(other.Genotype);
    }
    #endregion
    #endregion
}

