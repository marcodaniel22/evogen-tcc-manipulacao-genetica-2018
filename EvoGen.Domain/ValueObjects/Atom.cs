using EvoGen.Domain.Collections;
using System;

namespace EvoGen.Domain.ValueObjects
{
    public class Atom
    {
        public string Symbol { get; set; }
        public int Octet { get; set; }
        public int AtomId { get; set; }

        public Atom() { }

        public Atom(AtomNode atom)
        {
            this.Symbol = atom.Symbol;
            this.Octet = atom.Octet;
            this.AtomId = atom.AtomId;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.AtomId, this.Symbol);
        }
    }
}
