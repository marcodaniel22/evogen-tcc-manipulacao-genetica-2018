using EvoGen.Domain.GA.StructureGenerator;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Services;
using EvoGen.Repository.Repositories;
using Inject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoGen.Test.Domain.Services
{
    [TestClass]
    public class MoleculeServiceTests
    {
        private IMoleculeService _moleculeService;

        [TestInitialize]
        public void Initialize()
        {
            var container = new InjectContainer();
            container.Register<IMoleculeService, MoleculeService>();
            container.Register<IMoleculeRepository, MoleculeRepository>();
            container.Register<IAtomService, AtomService>();
            container.Register<ILinkService, LinkService>();

            _moleculeService = container.Resolve<IMoleculeService>();
        }

        [TestMethod]
        public void GetMoleculeCycles()
        {

        }
    }
}
