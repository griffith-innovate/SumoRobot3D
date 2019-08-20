using System;
using System.Collections.Generic;
public class Node {
    #region Public Members
    public Node( int id ) {
        ID = id;
        value = 0f;
        inConnections = new List<Connection>();
        outConnections = new List<Connection>();
    }

    public int GetID() {
        return ID;
    }

    public bool Ready() {
        bool ready = true;
        foreach ( Connection con in inConnections ) {
            if ( !con.GetStatus() ) {
                ready = false;
                break;
            }
        }
        return ready;
    }

    public void AddInConnection( Connection con ) {
        inConnections.Add( con );
    }

    public void AddOutConnection( Connection con ) {
        outConnections.Add( con );
    }

    public float GetValue() {
        return value;
    }

    public void SetValue( float val ) {
        value = ( float )Math.Tanh( val );
    }

    public void CalculateValue() {
        foreach ( Connection con in inConnections ) {
            value += con.GetValue();
        }
        value = ( float )Math.Tanh( value );
    }

    public void TransmitValue() {
        foreach ( Connection con in outConnections ) {
            con.SetValue( value );
        }
        value = 0;
    }
    #endregion
    #region Private Members
    private int ID;
    private float value;
    private List<Connection> inConnections;
    private List<Connection> outConnections;
    #endregion

}

    