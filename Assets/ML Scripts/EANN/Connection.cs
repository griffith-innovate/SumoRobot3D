using System;
using System.Collections.Generic;
public class Connection {
    #region Public Members

    public Connection( int input, int output, float weight ) {
        inNode = input;
        outNode = output;
        value = 0;
        ready = false;
        this.weight = weight;
    }

    public Connection( ConnectionGene con ) {
        inNode = con.GetInNode();
        outNode = con.GetOutNode();
        value = 0;
        ready = false;
        weight = con.GetWeight();
    }

    public int GetInNode() {
        return inNode;
    }

    public int GetOutNode() {
        return outNode;
    }

    public float GetValue() {
        float val = value;
        ready = false;
        value = 0;
        return val * weight;
    }

    public void SetValue( float val ) {
        value = val;
        ready = true;
    }

    public bool GetStatus() {
        return ready;
    }
    #endregion
    #region Private Members
    private int inNode;
    private int outNode;
    private float value;
    private float weight;
    private bool ready;
    #endregion
}