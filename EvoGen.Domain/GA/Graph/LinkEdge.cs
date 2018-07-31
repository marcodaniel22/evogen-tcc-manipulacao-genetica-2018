using System;
using System.Collections.Generic;

namespace EvoGen.Domain.Collections
{
    public class LinkEdge
    {
        public AtomNode From { get; set; }
        public AtomNode To { get; set; }

        public LinkEdge(AtomNode from, AtomNode to)
        {
            this.From = from;
            this.To = to;
        }

        public static List<LinkEdge> ListClone(List<LinkEdge> original)
        {
            List<LinkEdge> result = new List<LinkEdge>();
            foreach (var atom in original)
            {
                result.Add(new LinkEdge(
                    new AtomNode(atom.From.Symbol, atom.From.AtomId),
                    new AtomNode(atom.To.Symbol, atom.To.AtomId)
                ));
            }
            return result;
        }

        public override string ToString()
        {
            return String.Format("{0}-{1} => {2}-{3}", this.From.AtomId, this.From.Symbol, To.AtomId, this.To.Symbol);
        }
    }
}
