using System;

namespace EvoGen.Domain.ValueObjects
{
    public class Link
    {
        public Atom From { get; set; }
        public Atom To { get; set; }

        public Link() { }

        public Link(Atom from, Atom to)
        {
            this.From = from;
            this.To = to;
        }
        
        public override string ToString()
        {
            return String.Format("{0}-{1} => {2}-{3}", this.From.AtomId, this.From.Symbol, this.To.AtomId, this.To.Symbol);
        }
    }
}
