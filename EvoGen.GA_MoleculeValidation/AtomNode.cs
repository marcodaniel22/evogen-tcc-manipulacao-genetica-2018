using EvoGen.Helper;
using System;
using System.Linq;

namespace EvoGen.GA_MoleculeValidation
{
    public class AtomNode
    {
        public string Symbol { get; private set; }
        public int Octet { get; private set; }
        public int AtomId { get; private set; }
        public double AtomFitiness { get; private set; }

        private static Random random = new Random(DateTime.Now.Millisecond);

        public AtomNode(string symbol, int atomId)
        {
            this.Symbol = symbol;
            this.Octet = Util.OoctetRule[symbol];
            this.AtomId = atomId;
        }

        public void SetFitness(double fitness)
        {
            this.AtomFitiness = fitness;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.AtomId, this.Symbol);
        }
    }
}
