using EvoGen.Domain.Collections;
using EvoGen.Domain.GA.StructureGenerator;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Services;
using EvoGen.Domain.ValueObjects;
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
            container.Register<IAtomService, AtomService>();
            container.Register<ILinkService, LinkService>();

            _moleculeService = container.Resolve<IMoleculeService>();
        }

        [TestMethod]
        public void GetMoleculeCycles_Manual()
        {
            var molecule = new Molecule()
            {
                Nomenclature = "C4H5N3O",
                Links = new List<Link>(),
                AtomsCount = 13
            };
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 1, Symbol = "C" }, To = new Atom() { AtomId = 2, Symbol = "C" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 1, Symbol = "C" }, To = new Atom() { AtomId = 2, Symbol = "C" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 2, Symbol = "C" }, To = new Atom() { AtomId = 3, Symbol = "N" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 3, Symbol = "N" }, To = new Atom() { AtomId = 4, Symbol = "H" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 3, Symbol = "N" }, To = new Atom() { AtomId = 5, Symbol = "C" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 5, Symbol = "C" }, To = new Atom() { AtomId = 6, Symbol = "O" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 5, Symbol = "C" }, To = new Atom() { AtomId = 6, Symbol = "O" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 5, Symbol = "C" }, To = new Atom() { AtomId = 7, Symbol = "N" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 7, Symbol = "N" }, To = new Atom() { AtomId = 8, Symbol = "C" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 7, Symbol = "N" }, To = new Atom() { AtomId = 8, Symbol = "C" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 8, Symbol = "C" }, To = new Atom() { AtomId = 1, Symbol = "C" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 8, Symbol = "C" }, To = new Atom() { AtomId = 9, Symbol = "N" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 9, Symbol = "N" }, To = new Atom() { AtomId = 10, Symbol = "H" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 9, Symbol = "N" }, To = new Atom() { AtomId = 11, Symbol = "H" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 1, Symbol = "C" }, To = new Atom() { AtomId = 12, Symbol = "H" } });
            molecule.Links.Add(new Link() { From = new Atom() { AtomId = 2, Symbol = "C" }, To = new Atom() { AtomId = 13, Symbol = "H" } });

            var cycles = _moleculeService.GetMoleculeCycles(molecule);
        }

        [TestMethod]
        public void GetMoleculeCycles_Auto()
        {
            var cytosineList = _moleculeService.GetAll().Where(x => x.Nomenclature == "C4H5N3O").ToList();
            foreach (var molecule in cytosineList)
            {
                var cycles = _moleculeService.GetMoleculeCycles(molecule);
            }
        }
    }
}
