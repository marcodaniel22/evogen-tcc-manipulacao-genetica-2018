using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Interfaces.Services.Reaction;
using EvoGen.Domain.Services;
using EvoGen.Domain.Services.Reactions;
using EvoGen.Domain.ValueObjects.DNA;
using EvoGen.Repository.Repositories;
using Inject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace EvoGen.Test.Domain.Services
{
    [TestClass]
    public class ReplacementReactionServiceTests
    {
        private IReplacementReactionService _replacementReactionService;

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
            container.Register<IReplacementReactionService, ReplacementReactionService>();
            container.Register<IAddictionReactionService, AddictionReactionService>();

            _replacementReactionService = container.Resolve<IReplacementReactionService>();
        }

        [TestMethod]
        public void Cytosine_Reaction()
        {
            var cytosine = new Cytosine();
            var substract = cytosine.GetTestSubstract();
            var reagent = cytosine.GetTestReagent();

            var substractLinks = substract.Links.ToList();
            var result = _replacementReactionService.React(reagent, substract);
            Assert.AreNotEqual(substractLinks, result.Links);

        }
    }
}
