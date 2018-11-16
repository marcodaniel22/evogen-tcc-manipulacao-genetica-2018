using EvoGen.Domain.Collections;
using System.Collections.Generic;

namespace EvoGen.Domain.ValueObjects.FakeDNA
{
    public class FakeAdenine1 : Molecule
    {
        public FakeAdenine1()
        {
            this.Nomenclature = "C5H3N4Cl";
            this.AtomsCount = 13;
            this.DiferentAtomsCount = 4;
            this.Atoms = new List<Atom>
            {
                new Atom() { AtomId = 1, Octet = 4, Symbol = "C" },
                new Atom() { AtomId = 2, Octet = 4, Symbol = "C" },
                new Atom() { AtomId = 3, Octet = 4, Symbol = "C" },
                new Atom() { AtomId = 4, Octet = 4, Symbol = "C" },
                new Atom() { AtomId = 5, Octet = 4, Symbol = "C" },
                new Atom() { AtomId = 6, Octet = 5, Symbol = "N" },
                new Atom() { AtomId = 7, Octet = 5, Symbol = "N" },
                new Atom() { AtomId = 8, Octet = 5, Symbol = "N" },
                new Atom() { AtomId = 9, Octet = 5, Symbol = "N" },
                new Atom() { AtomId = 10, Octet = 7, Symbol = "Cl" },
                new Atom() { AtomId = 11, Octet = 1, Symbol = "H" },
                new Atom() { AtomId = 12, Octet = 1, Symbol = "H" },
                new Atom() { AtomId = 13, Octet = 1, Symbol = "H" }
            };
            this.Links = new List<Link>
            {
                new Link(GetAtomById(1), GetAtomById(11)),
                new Link(GetAtomById(1), GetAtomById(7)),
                new Link(GetAtomById(7), GetAtomById(12)),
                new Link(GetAtomById(7), GetAtomById(3)),
                new Link(GetAtomById(3), GetAtomById(2)),
                new Link(GetAtomById(3), GetAtomById(2)),
                new Link(GetAtomById(2), GetAtomById(6)),
                new Link(GetAtomById(6), GetAtomById(1)),
                new Link(GetAtomById(6), GetAtomById(1)),
                new Link(GetAtomById(2), GetAtomById(5)),
                new Link(GetAtomById(5), GetAtomById(9)),
                new Link(GetAtomById(5), GetAtomById(9)),
                new Link(GetAtomById(9), GetAtomById(4)),
                new Link(GetAtomById(4), GetAtomById(13)),
                new Link(GetAtomById(4), GetAtomById(8)),
                new Link(GetAtomById(4), GetAtomById(8)),
                new Link(GetAtomById(8), GetAtomById(3)),
                new Link(GetAtomById(5), GetAtomById(10))
            };
        }

        private Atom GetAtomById(int id)
        {
            return this.Atoms.Find(x => x.AtomId == id);
        }
    }
}
