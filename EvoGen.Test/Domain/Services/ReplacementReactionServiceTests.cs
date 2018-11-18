using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Interfaces.Services.Reaction;
using EvoGen.Domain.Services;
using EvoGen.Domain.Services.Reactions;
using EvoGen.Domain.ValueObjects;
using EvoGen.Domain.ValueObjects.DNA;
using EvoGen.Domain.ValueObjects.MutatedDNA;
using EvoGen.Repository.Repositories;
using Inject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            var substract = new MutatedCytosine1();
            var reagent = GetReagent();
            
            var result = _replacementReactionService.React(reagent, substract);
            Assert.AreNotEqual(substract.Links, result.Links);

        }

        [TestMethod]
        public void Adenine_Reaction()
        {
            var adenine = new Adenine();
            var substract = new MutatedAdenine1();
            var reagent = GetReagent();
            
            var result = _replacementReactionService.React(reagent, substract);
            Assert.AreNotEqual(substract.Links, result.Links);

        }

        private Molecule GetReagent()
        {
            var molecule = new Molecule();
            molecule.Nomenclature = "NH3";
            molecule.AtomsCount = 4;
            molecule.DiferentAtomsCount = 2;
            molecule.Atoms = new List<Atom>
            {
                new Atom() { AtomId = 1, Octet = 5, Symbol = "N" },
                new Atom() { AtomId = 2, Octet = 1, Symbol = "H" },
                new Atom() { AtomId = 3, Octet = 1, Symbol = "H" },
                new Atom() { AtomId = 4, Octet = 1, Symbol = "H" }
            };
            molecule.Links = new List<Link>
            {
                new Link(GetAtomById(molecule.Atoms, 1), GetAtomById(molecule.Atoms, 2)),
                new Link(GetAtomById(molecule.Atoms, 1), GetAtomById(molecule.Atoms, 3)),
                new Link(GetAtomById(molecule.Atoms, 1), GetAtomById(molecule.Atoms, 4))
            };
            return molecule;
        }

        private Atom GetAtomById(List<Atom> atoms, int id)
        {
            return atoms.Find(x => x.AtomId == id);
        }
    }
}
}
