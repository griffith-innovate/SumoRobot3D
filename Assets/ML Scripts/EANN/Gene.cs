using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NodeGene
public class NodeGene {
    #region Public Members
    public enum TYPE{
        INPUT, OUTPUT, HIDDEN
    };

    public NodeGene(int id, TYPE type){
        ID = id;
        this.type = type;
    }

    public NodeGene(NodeGene copy){
        ID = copy.ID;
        type = copy.type;
    }

    public int GetID(){
        return ID;
    }

    public TYPE GetNodeType(){
        return type;
    }
    #endregion
    #region Private Members
    private int ID;                                         //Node ID
    private TYPE type;    
    #endregion                                  //Node type
}

public class ConnectionGene                                 //ConnectionGene Subclass
{
    #region Public Members

    public ConnectionGene(int inNode, int outNode, float weight, bool expressed, int innovation){
        this.inNode = inNode;
        this.outNode = outNode;
        this.weight = weight;
        this.expressed = expressed;
        this.innovation = innovation;
    }

    public ConnectionGene(ConnectionGene copy){
        inNode = copy.GetInNode();
        outNode = copy.GetOutNode();
        weight = copy.GetWeight();
        expressed = copy.IsExpressed();
        innovation = copy.GetInnovation();
    }

    public void Disable(){
        expressed = false;
    }

    public void RandomWeight(Random r){
        weight = (float)(r.NextDouble()*2-1);
    }

    public void PerturbWeight(Random r){
        weight += (float)(r.NextDouble()-0.5)*0.5f;
    }

    public int GetInNode(){
        return inNode;
    }

    public int GetOutNode(){
        return outNode;
    }

    public float GetWeight(){
        return weight;
    }

    public bool IsExpressed(){
        return expressed;
    }

    public int GetInnovation(){
        return innovation;
    }
    #endregion
    #region Private Members
    private int inNode;         //input node
    private int outNode;        //output node 
    private float weight;       //connection weight
    private bool expressed;     //is the connection enabled or disabled
    private int innovation;     //innovation number of connection
    #endregion
}