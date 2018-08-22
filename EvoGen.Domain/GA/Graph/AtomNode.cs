using EvoGen.Helper;
using System;
using System.Collections.Generic;

namespace EvoGen.Domain.Collections
{
    public class AtomNode
    {
        public string Symbol { get; private set; }
        public int Octet { get; private set; }
        public int AtomId { get; private set; }
        public double AtomFitiness { get; private set; }

        public AtomNode(string symbol, int atomId)
        {
            this.Symbol = symbol;
            this.Octet = Constants.OoctetRule[symbol];
            this.AtomId = atomId;
        }

        public void SetFitness(double fitness)
        {
            this.AtomFitiness = fitness;
        }

        public static List<AtomNode> ListClone(List<AtomNode> original)
        {
            List<AtomNode> result = new List<AtomNode>();
            foreach (var atom in original)
            {
                result.Add(new AtomNode(atom.Symbol, atom.AtomId));
            }
            return result;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.AtomId, this.Symbol);
        }
    }
}
