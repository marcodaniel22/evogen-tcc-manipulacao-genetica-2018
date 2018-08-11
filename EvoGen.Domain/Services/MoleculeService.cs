using System;
using System.Collections.Generic;
using System.Linq;
using EvoGen.Domain.Collections;
using EvoGen.Domain.Collections.ValueObjects;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;

namespace EvoGen.Domain.Services
{
    public class MoleculeService : ServiceBase<Molecule>, IMoleculeService
    {
        private readonly IMoleculeRepository _moleculeRepository;
        private readonly IAtomService _atomService;
        private readonly ILinkService _linkService;

        public MoleculeService(IMoleculeRepository moleculeRepository, IAtomService atomService, ILinkService linkService)
            : base(moleculeRepository)
        {
            this._moleculeRepository = moleculeRepository;
            this._atomService = atomService;
            this._linkService = linkService;
        }

        public Molecule Create(MoleculeGraph molecule)
        {
            return Create(GetCollectionFromGraph(molecule));
        }

        public int MoleculeCount()
        {
            return _moleculeRepository.GetAll().Count();
        }

        public Molecule GetCollectionFromGraph(MoleculeGraph molecule)
        {
            var collection = new Molecule();
            collection.guidString = Guid.NewGuid().ToString();
            collection.Nomenclature = molecule.Nomenclature;
            collection.IdStructure = molecule.IdStructure;
            collection.AtomsCount = molecule.AtomNodes.Count;
            collection.Links = new List<Link>();
            foreach (var link in molecule.LinkEdges)
            {
                collection.Links.Add(_linkService.GetCollectionFromEdge(link));
            }
            return collection;

        }

        public Molecule GetByIdStructure(string nomenclature, string idStructure)
        {
            return _moleculeRepository.GetAll()
                .FirstOrDefault(x => x.Nomenclature == nomenclature && x.IdStructure == idStructure);
        }
    }
}
