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
    public class GATests
    {
        [TestMethod]
        public void GA_FindSolution()
        {
            GA ga = new GA("C4H5N3O", 1000, 2000, 0.20);
            // GA ga = new GA("H18C8", 1000, 2000, 0.20);
            // GA ga = new GA("C2H6O", 1000, 2000, 0.20);
            MoleculeGraph molecule = ga.FindSolution();
        }
    }
}
