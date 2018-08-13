using System;
using System.Collections.Generic;
using System.Linq;
using EvoGen.Domain.Collections;
using EvoGen.Domain.ValueObjects;
using EvoGen.Domain.Interfaces.Repositories;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Helper;

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
            collection.Links = molecule.LinkEdges.Select(x => _linkService.GetCollectionFromEdge(x)).ToList();
            return collection;

        }

        public Molecule GetByIdStructure(string nomenclature, string idStructure)
        {
            return _moleculeRepository.GetAll()
                .FirstOrDefault(x => x.Nomenclature == nomenclature && x.IdStructure == idStructure);
        }

        public List<Cycle> GetMoleculeCycles(Molecule molecule)
        {
            List<Cycle> cycles = new List<Cycle>();
            var links = molecule.Links.Where(x => Constants.CyclicCompoundAtoms.Contains(x.From.Symbol) && Constants.CyclicCompoundAtoms.Contains(x.To.Symbol)).ToList();
            foreach (var link in links)
            {
                var visited = new List<Atom>();
                var stack = new Stack<Node>();
                var firstAtom = link.From;

                Node atomNode = new Node();
                atomNode.Value = firstAtom;
                stack.Push(atomNode);

                while (stack.Count > 0)
                {
                    atomNode = stack.Pop();
                    if (!visited.Contains(atomNode.Value))
                    {
                        visited.Add(atomNode.Value);
                        var atomLinks = _linkService.GetLinksFromAtom(atomNode.Value, links);
                        foreach (var atomLink in atomLinks)
                        {
                            var newNode = new Node();
                            newNode.Parent = atomNode;
                            newNode.Value = atomLink.To;

                            if (newNode.Value == firstAtom)
                            {
                                atomNode = newNode;
                                break;
                            }
                            else
                            {
                                stack.Push(newNode);
                            }
                        }
                    }
                }
            }

            return new List<Cycle>();
        }
    }
}
