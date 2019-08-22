#region Includes
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
#endregion


/*
================================================================================
Class representing one member of a population

Original code by Samuel Arzt (March 2017) at:
https://github.com/ArztSamuel/Applying_EANNs
================================================================================
 */

public class Genotype : IComparable<Genotype>, IEnumerable<float> {
    #region Members
    private static Random randomizer = new Random();


    // The current evaluation of this genotype.
    public float Evaluation { get; set; }

    // The current fitness (e.g, the evaluation of this genotype relative 
    // to the average evaluation of the whole population) of this genotype.
    public float Fitness { get; set; }

    // The vector of parameters of this genotype.
    private float[] parameters;


    // The amount of parameters stored in the parameter vector of this genotype.
    public int ParameterCount {
        get {
            if (parameters == null) return 0;
            return parameters.Length;
        }
    }

    // Overridden indexer for convenient parameter access.
    public float this[int index] {
        get { return parameters[index]; }
        set { parameters[index] = value; }
    }
    #endregion

    #region Constructors

    // Instance of a new genotype with given parameter vector and initial fitness of 0.
    public Genotype(float[] parameters) {
        this.parameters = parameters;
        Fitness = 0;
    }
    #endregion

    #region Methods
    #region IComparable

    // Compares this genotype with another genotype depending on their fitness values.
    public int CompareTo(Genotype other) {
        return other.Fitness.CompareTo(this.Fitness); //in reverse order for larger fitness being first in list
    }
    #endregion

    #region IEnumerable

    // Gets an Enumerator to iterate over all parameters of this genotype.
    public IEnumerator<float> GetEnumerator() {
        for (int i = 0; i < parameters.Length; i++)
            yield return parameters[i];
    }


    // Gets an Enumerator to iterate over all parameters of this genotype.
    IEnumerator IEnumerable.GetEnumerator() {
        for (int i = 0; i < parameters.Length; i++)
            yield return parameters[i];
    }
    #endregion


    // Sets the parameters of this genotype to random values in given range.
    public void SetRandomParameters(float minValue, float maxValue) {
        //Check arguments
        if (minValue > maxValue) throw new ArgumentException("Minimum value may not exceed maximum value.");

        //Generate random parameter vector
        float range = maxValue - minValue;
        for (int i = 0; i < parameters.Length; i++)
            parameters[i] = (float)((randomizer.NextDouble() * range) + minValue); //Create a random float between minValue and maxValue
    }


    // Returns a copy of the parameter vector.

    public float[] GetParameterCopy() {
        float[] copy = new float[ParameterCount];
        for (int i = 0; i < ParameterCount; i++)
            copy[i] = parameters[i];

        return copy;
    }


    // Saves the parameters of this genotype to a file at given file path.
    public void SaveToFile(string filePath) {
        StringBuilder builder = new StringBuilder();
        foreach (float param in parameters)
            builder.Append(param.ToString()).Append(";");

        builder.Remove(builder.Length - 1, 1);

        File.WriteAllText(filePath, builder.ToString());
    }

    #region Static Methods

    // Generates a random genotype with parameters in given range.
    public static Genotype GenerateRandom(uint parameterCount, float minValue, float maxValue) {
        //Check arguments
        if (parameterCount == 0) return new Genotype(new float[0]);

        Genotype randomGenotype = new Genotype(new float[parameterCount]);
        randomGenotype.SetRandomParameters(minValue, maxValue);

        return randomGenotype;
    }


    // Loads a genotype from a file with given file path.
    public static Genotype LoadFromFile(string filePath) {
        string data = File.ReadAllText(filePath);

        List<float> parameters = new List<float>();
        string[] paramStrings = data.Split(';');

        foreach (string parameter in paramStrings) {
            float parsed;
            if (!float.TryParse(parameter, out parsed)) throw new ArgumentException("The file at given file path does not contain a valid genotype serialisation.");
            parameters.Add(parsed);
        }

        return new Genotype(parameters.ToArray());
    }
    #endregion
    #endregion
}
