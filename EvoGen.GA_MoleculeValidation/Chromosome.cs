using EvoGen.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EvoGen.GA_MoleculeValidation
{
    public class Chromosome
    {
        public MoleculeGraph Molecule { get; private set; }
        public double Fitness { get; private set; }

        private static Random random = new Random(DateTime.Now.Millisecond);

        public Chromosome(MoleculeGraph molecule)
        {
            this.Molecule = molecule;
            this.CalcFitness();
        }

        private void CalcFitness()
        {
            this.Fitness = 0;
            foreach (var atom in this.Molecule.AtomNodes)
            {
                double atomFitness = 0;
                var atomLinks = this.Molecule.GetLinksFromAtom(atom);
                // Rule 1
                var v = atom.Octet;
                var l = atom.Octet - atomLinks.Count;
                l = l < 0 ? 0 : l;
                var s = atomLinks.Count * 2;
                var result = (v - (l + (s / 2)));
                atomFitness += Math.Abs(result);

                // Rule 2
                if (atom.Symbol == "H")
                {
                    atomFitness += Math.Abs(2 - (v + atomLinks.Count));
                    atomFitness += atomLinks.Count(x => x.To.Symbol == "H");
                }
                else
                    atomFitness += Math.Abs(8 - (v + atomLinks.Count));

                atom.SetFitness(atomFitness);
            }

            // Rule 3
            if (((this.Molecule.AtomNodes.Count - 1) - this.Molecule.LinkEdges.Count) > 0)
                this.Fitness += (this.Molecule.AtomNodes.Count - 1) - this.Molecule.LinkEdges.Count;

            // Rule 4
            this.Fitness += Math.Abs(this.Molecule.AtomNodes.Count - this.Molecule.InterconectedAtoms());

            this.Fitness += this.Molecule.AtomNodes.Sum(x => x.AtomFitiness);
        }

        public Chromosome Crossover(Chromosome pair)
        {
            List<LinkEdge> childLinks = new List<LinkEdge>();
            childLinks.AddRange(LinkEdge.ListClone(this.Molecule.LinkEdges.Take(this.Molecule.LinkEdges.Count / 2).ToList()));
            childLinks.AddRange(LinkEdge.ListClone(pair.Molecule.LinkEdges.Skip(childLinks.Count).ToList()));
            
            return new Chromosome(new MoleculeGraph(this.Molecule.Nomenclature, this.Molecule.AtomNodes, childLinks));
        }

        public void Mutate(double rate)
        {
            double randonrate = random.NextDouble();
            if (randonrate < rate)
            {
                bool mutated = false;
                do
                {
                    var link1 = this.Molecule.LinkEdges[random.Next(this.Molecule.LinkEdges.Count)];
                    var link2 = this.Molecule.LinkEdges[random.Next(this.Molecule.LinkEdges.Count)];
                    mutated = (link1.From.AtomId != link2.To.AtomId) && (link2.From.AtomId != link1.To.AtomId);
                    if (mutated)
                    {
                        var bag = link1.To;
                        link1.To = link2.To;
                        link2.To = bag;
                    }
                } while (!mutated);

                if ((this.Molecule.AtomNodes.Count - 1) < this.Molecule.LinkEdges.Count && random.NextDouble() > 0.90)
                    this.Molecule.LinkEdges.RemoveAt(random.Next(this.Molecule.LinkEdges.Count));

                this.CalcFitness();
            }
        }
    }
}
