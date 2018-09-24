using EvoGen.Domain.Collections;
using EvoGen.Domain.GA.StructureGenerator;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Services;
using EvoGen.Repository.Repositories;
using Inject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoGen.Test.Domain.GA
{
    [TestClass]
    public class MoleculeGraphTests
    {
        private ILinkService _linkService;

        [TestInitialize]
        public void Initialize()
        {
            var container = new InjectContainer();
            container.Register<IMoleculeService, MoleculeService>();
            container.Register<IMoleculeRepository, MoleculeRepository>();
            container.Register<ILogService, LogService>();
            container.Register<ILogRepository, LogRepository>();
            container.Register<IAtomService, AtomService>();
            container.Register<ILinkService, LinkService>();

            _linkService = container.Resolve<ILinkService>();
        }

        [TestMethod]
        public void GetIdStructure()
        {
            var ga1 = new StructureGenerator("H2O", 100, 2000, 0.20);
            var molecule1 = ga1.FindSolution();
            var idStructure1 = _linkService.GetIdStructure(molecule1.LinkEdges);
            Assert.AreEqual("HO2-OH2", idStructure1);

            var ga2 = new StructureGenerator("C2H6O", 100, 2000, 0.20);
            var molecule2 = ga2.FindSolution();
            var idStructure2 = _linkService.GetIdStructure(molecule2.LinkEdges);
            Assert.AreEqual("CC2-CH5-CO1-HC5-HO1-OC1-OH1", idStructure2);
        }
    }
}
