using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EANN : MonoBehaviour
{
    #region Public Variables
    public int MaxGenerations { get; set; } = 100;                              // Maximum number of generations
    public int PopulationSize { get; set; } = 50;                               // The number of individuals in a pop
    #endregion

    #region Private Variables
    [SerializeField] private int maxGenerations;
    [SerializeField] private int populationSize;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private Genotype NewGenotype()
    {
        Genotype child = new Genotype();
        return child;
    }

    public Crossover(Genotype parentA, Genotype parentB)
    {
        if (parentB.Fitness > parentA.Fitness)
        {
            Genotype temp = parentA;
            parentA = parentB;
            parentB = temp;
        }

        Genotype child = NewGenotype();

        List<innovation> innovations = new List<innovation>();

    }
}
