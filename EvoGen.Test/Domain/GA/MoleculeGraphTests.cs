using EvoGen.Domain.Collections;
using EvoGen.Domain.GA.StructureGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EvoGen.Test.Domain.GA
{
    [TestClass]
    public class MoleculeGraphTests
    {
        [TestMethod]
        public void GetIdStructure()
        {
            var ga1 = new StructureGenerator("H2O", 100, 2000, 0.20);
            var molecule1 = ga1.FindSolution();
            var idStructure1 = MoleculeGraph.GetIdStructure(molecule1.LinkEdges);
            Assert.AreEqual("HO2-OH2", idStructure1);

            var ga2 = new StructureGenerator("C2H6O", 100, 2000, 0.20);
            var molecule2 = ga2.FindSolution();
            var idStructure2 = MoleculeGraph.GetIdStructure(molecule2.LinkEdges);
            Assert.AreEqual("CC2-CH5-CO1-HC5-HO1-OC1-OH1", idStructure2);
        }
    }
}
