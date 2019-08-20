namespace SumoRobot3D.Assets.ML_Scripts.EANN
{
    public class NodeGene
    {
        public int ID;
        public int Type;            // 0: sensor, 1: hidden, 2: output

        public NodeGene(int ID, int type)
        {
            this.ID = ID;
            this.type = type;
        }
    }
    public class ConnectionGene
    {
        public int In;
        public int Out;
        public float Weight;
        public bool Enabled;
        public int Innovation;

        #region Constructors
        public ConnectionGene(int In, int Out)
        {
            this.In = In;
            this.Out = Out;
            this.Weight = Random.Range(-1.0f, 1.0f);
            this.Enabled = true;
            this.Innovation = 1;
        }
        public ConnectionGene(int In, int Out, int Innovation)
        {
            this.In = In;
            this.Out = Out;
            this.Weight = Random.Range(-1.0f, 1.0f);
            this.Enabled = true;
            this.Innovation = Innovtaion;
        }
        public ConnectionGene(int In, int Out, int Innovation, float Weight)
        {
            this.In = In;
            this.Out = Out;
            this.Weight = Random.Range(-1.0f, 1.0f);
            this.Enabled = true;
            this.Innovation = Innovtaion;
            this.Weight = Weight;
        }
        #endregion
    }
}