namespace SumoRobot3D.Assets.ML_Scripts.EANN
{
    public class Genotype
    {
        public List<NodeGene> NodeGenes;
        public List<ConnectionGene> ConnectionGenes;

        public Genotype(int inputs, int outputs)
        {
            NodeGenes = new List<NodeGene>();
            ConnectionGenes = new List<ConnectionGene>();

            // Create all the node genes
            for (int i = 0; i < inputs + outputs; i++)
            {
                int type = 0;
                if (i >= inputs)
                {
                    type = 2;
                }
                NodeGene newNode = NodeGene(i, type);
                NodeGenes.Add(newNode);
            }

            // Create the connections
            for (int i = 0; i < inputs; i++)
            {
                NodeGene fromNode = NodeGenes[i];
                for (int j = inputs; j < inputs + outputs; j++)
                {
                    NodeGene toNode = NodeGenes[j];

                    // Create connection between the two nodes
                    ConnectionGene = new ConnectionGene(fromNode.ID, toNode.ID);
                    ConnectionGenes.Add(ConnectionGene);
                }
            }
        }
    }
}