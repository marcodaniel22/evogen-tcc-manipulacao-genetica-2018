using EvoGen.Domain.Collections;
using EvoGen.Domain.GA.StructureGenerator;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Services;
using EvoGen.Domain.ValueObjects;
using EvoGen.Domain.ValueObjects.DNA;
using EvoGen.Repository.Repositories;
using Inject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

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
            container.Register<ILogService, LogService>();
            container.Register<ILogRepository, LogRepository>();
            container.Register<IAtomService, AtomService>();
            container.Register<ILinkService, LinkService>();
            container.Register<IReactionService, ReactionService>();

            _moleculeService = container.Resolve<IMoleculeService>();
        }

        [TestMethod]
        public void GetMoleculeCycles_Adenine()
        {
            var adenine = new Adenine();
            var cycles = _moleculeService.GetMoleculeCycles(adenine);
            Assert.AreEqual(2, cycles.Count);
        }

        [TestMethod]
        public void GetMoleculeCycles_Cytosine()
        {
            var cytosine = new Cytosine();
            var cycles = _moleculeService.GetMoleculeCycles(cytosine);
            Assert.AreEqual(1, cycles.Count);
        }

        [TestMethod]
        public void GetMoleculeCycles_Guanine()
        {
            var guanine = new Guanine();
            var cycles = _moleculeService.GetMoleculeCycles(guanine);
            Assert.AreEqual(2, cycles.Count);
        }

        [TestMethod]
        public void GetMoleculeCycles_Thymine()
        {
            var thymine = new Thymine();
            var cycles = _moleculeService.GetMoleculeCycles(thymine);
            Assert.AreEqual(1, cycles.Count);
        }
    }
}
