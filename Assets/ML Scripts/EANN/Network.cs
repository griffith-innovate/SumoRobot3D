using System;
using System.Collections.Generic;

public class Network : IComparable<Network> {
    #region Public Members
    
    public Network( Genome gen ) {
        connections = new List<Connection>();
        nodes = new List<Node>();
        inputNodes = new List<Node>();
        outputNodes = new List<Node>();
        hiddenNodes = new List<Node>();
        genome = gen;
        nodeGenes = genome.GetNodes();
        connectionGenes = genome.GetConnections();
        foreach ( Genome.ConnectionGene con in connectionGenes.Values ) {
            if ( con.IsExpressed() ) {
                Connection newCon = new Connection(con);
                connections.Add( newCon );
            }
        }
        MakeNetwork();
    }

    

    public void MakeNetwork() {
        foreach ( Genome.NodeGene nodeGene in nodeGenes ) {
            Node node = new Node(nodeGene.GetID());
            nodes.Add( node );
            if ( nodeGene.GetNodeType() == Genome.NodeGene.TYPE.INPUT ) {
                inputNodes.Add( node );
            } else if ( nodeGene.GetNodeType() == Genome.NodeGene.TYPE.OUTPUT ) {
                outputNodes.Add( node );
            } else {
                hiddenNodes.Add( node );
            }
        }

        foreach ( Node node in nodes ) {
            foreach ( Connection con in connections ) {
                if ( con.GetInNode() == node.GetID() ) {
                    node.AddOutConnection( con );
                } else if ( con.GetOutNode() == node.GetID() ) {
                    node.AddInConnection( con );
                }
            }
        }
    }

    public float[] GetOutput( float[] input ) {
        float[] output = new float[outputNodes.Count];
        for ( int i = 0; i < inputNodes.Count; i++ ) {
            inputNodes[i].SetValue( input[i] );
            inputNodes[i].TransmitValue();
        }

        List<Node> copyList = new List<Node>(hiddenNodes);

        while ( copyList.Count != 0 ) {
            List<Node> removeNodes = new List<Node>();
            foreach ( Node node in copyList ) {
                if ( node.Ready() ) {
                    node.CalculateValue();
                    node.TransmitValue();
                    removeNodes.Add( node );
                }
            }

            foreach ( Node node in removeNodes ) {
                copyList.Remove( node );
            }
        }

        for ( int i = 0; i < outputNodes.Count; i++ ) {
            outputNodes[i].CalculateValue();
            output[i] = outputNodes[i].GetValue();
        }

        return output;
    }

    public Genome GetGenome() {
        return genome;
    }

    public float GetFitness() {
        return fitness;
    }

    public void SetFitness( float fit ) {
        fitness = fit;
    }

    public void AddFitness( float fit ) {
        fitness += fit;
    }

    public int CompareTo( Network other ) {
        return other.GetFitness().CompareTo( fitness );
    }
    #endregion
    #region Private Members
    private Genome genome;
    private List<NodeGene> nodeGenes;
    private Dictionary<int, ConnectionGene> connectionGenes;
    private List<Node> nodes;
    private List<Node> inputNodes;
    private List<Node> outputNodes;
    private List<Node> hiddenNodes;
    private List<Connection> connections;
    private float fitness;
    #endregion

}