
using EvoGen.Domain.Collections;
using System.Collections.Generic;

namespace EvoGen.Domain.ValueObjects.MutatedDNA
{
    public class MutatedThymine1 : Molecule
    {
        public MutatedThymine1()
        {
            this.Nomenclature = "C4H3O2N2Br";
            this.AtomsCount = 12;
            this.DiferentAtomsCount = 5;
            this.Atoms = new List<Atom>
            {
                new Atom() { AtomId = 1, Octet = 4, Symbol = "C" },
                new Atom() { AtomId = 2, Octet = 4, Symbol = "C" },
                new Atom() { AtomId = 3, Octet = 4, Symbol = "C" },
                new Atom() { AtomId = 4, Octet = 4, Symbol = "C" },
                new Atom() { AtomId = 5, Octet = 7, Symbol = "Br" },
                new Atom() { AtomId = 6, Octet = 5, Symbol = "N" },
                new Atom() { AtomId = 7, Octet = 5, Symbol = "N" },
                new Atom() { AtomId = 8, Octet = 6, Symbol = "O" },
                new Atom() { AtomId = 9, Octet = 6, Symbol = "O" },
                new Atom() { AtomId = 10, Octet = 1, Symbol = "H" },
                new Atom() { AtomId = 11, Octet = 1, Symbol = "H" },
                new Atom() { AtomId = 12, Octet = 1, Symbol = "H" }
            };
            this.Links = new List<Link>
            {
                new Link(GetAtomById(1), GetAtomById(8)),
                new Link(GetAtomById(1), GetAtomById(8)),
                new Link(GetAtomById(1), GetAtomById(6)),
                new Link(GetAtomById(6), GetAtomById(10)),
                new Link(GetAtomById(6), GetAtomById(2)),
                new Link(GetAtomById(2), GetAtomById(11)),
                new Link(GetAtomById(2), GetAtomById(3)),
                new Link(GetAtomById(2), GetAtomById(3)),
                new Link(GetAtomById(3), GetAtomById(4)),
                new Link(GetAtomById(4), GetAtomById(9)),
                new Link(GetAtomById(4), GetAtomById(9)),
                new Link(GetAtomById(4), GetAtomById(7)),
                new Link(GetAtomById(7), GetAtomById(12)),
                new Link(GetAtomById(7), GetAtomById(1)),
                new Link(GetAtomById(3), GetAtomById(5))
            };
        }

        private Atom GetAtomById(int id)
        {
            return this.Atoms.Find(x => x.AtomId == id);
        }
    }
}
