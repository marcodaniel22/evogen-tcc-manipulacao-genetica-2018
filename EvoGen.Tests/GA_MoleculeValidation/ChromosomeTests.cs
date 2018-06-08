using EvoGen.GA_MoleculeValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EvoGen.GA_MoleculeValidation.Chromosome;

namespace EvoGen.Tests.GA_MoleculeValidation
{
    [TestClass]
    public class ChromosomeTests
    {
        [TestMethod]
        public void MoleculeGraph_ExtractAtomsFromNomenclature()
        {
            List<AtomNode> atoms;

            atoms = MoleculeGraph.ExtractAtomsFromNomenclature("H2O");
            Assert.AreEqual(3, atoms.Count);

            atoms = MoleculeGraph.ExtractAtomsFromNomenclature("H2OCl2N51");
            Assert.AreEqual(56, atoms.Count);
        }

        [TestMethod]
        public void MoleculeGraph_GenerateRandomLinks()
        {
            MoleculeGraph molGraph;
            List<AtomNode> atoms;

            atoms = MoleculeGraph.ExtractAtomsFromNomenclature("H2O");
            molGraph = new MoleculeGraph("H2O", atoms);
            Assert.IsTrue(molGraph.LinkEdges.Count >= 1);

            atoms = MoleculeGraph.ExtractAtomsFromNomenclature("H2OCl2N51");
            Assert.IsTrue(molGraph.LinkEdges.Count >= 1);
        }

        [TestMethod]
        public void Chromosome_Crossover()
        {
            Chromosome parent;
            Chromosome pair;
            Chromosome child;
            List<AtomNode> atoms;

            atoms = MoleculeGraph.ExtractAtomsFromNomenclature("H2O");
            parent = new Chromosome(new MoleculeGraph("H2O", atoms));
            pair = new Chromosome(new MoleculeGraph("H2O", atoms));
            child = parent.Crossover(pair);
            Assert.IsTrue(child.Molecule.LinkEdges.Count > 0);

            atoms = MoleculeGraph.ExtractAtomsFromNomenclature("H2OCl2N51");
            parent = new Chromosome(new MoleculeGraph("H2OCl2N51", atoms));
            pair = new Chromosome(new MoleculeGraph("H2OCl2N51", atoms));
            child = parent.Crossover(pair);
            Assert.IsTrue(child.Molecule.LinkEdges.Count > 0);
        }

        [TestMethod]
        public void Chromosome_Mutate()
        {
            Chromosome target;
            List<AtomNode> atoms;
            double mutationRate = 0.20;

            atoms = MoleculeGraph.ExtractAtomsFromNomenclature("H2O");
            target = new Chromosome(new MoleculeGraph("H2O", atoms));
            target.Mutate(mutationRate);
            Assert.IsTrue(target.Molecule.LinkEdges.Count > 0);

            atoms = MoleculeGraph.ExtractAtomsFromNomenclature("H2O");
            target = new Chromosome(new MoleculeGraph("H2OCl2N51", atoms));
            target.Mutate(mutationRate);
            Assert.IsTrue(target.Molecule.LinkEdges.Count > 0);
        }
    }
}
