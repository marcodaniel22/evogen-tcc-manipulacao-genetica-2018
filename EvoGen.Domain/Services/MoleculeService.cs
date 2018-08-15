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
                var cycled = false;
                var visited = new List<Atom>();
                var queue = new Queue<Node<Atom>>();
                var firstAtom = link.From;

                var atomNode = new Node<Atom>();
                atomNode.Value = firstAtom;
                queue.Enqueue(atomNode);

                while (queue.Count > 0)
                {
                    atomNode = queue.Dequeue();
                    if (!visited.Any(x => x.AtomId == atomNode.Value.AtomId))
                    {
                        visited.Add(atomNode.Value);
                        var atoms = links.Where(x => x.From.AtomId == atomNode.Value.AtomId).Select(x => x.To).ToList();
                        var neighbors = atoms.Select(x => new Node<Atom>()
                        {
                            Parent = atomNode,
                            Value = x
                        }).ToList();

                        if (neighbors.Where(x => x.Value.AtomId == firstAtom.AtomId).Any(x => x.Parent.Parent.Value.AtomId != firstAtom.AtomId))
                        {
                            atomNode = neighbors.FirstOrDefault(x => x.Value.AtomId == firstAtom.AtomId);
                            cycled = true;
                            break;
                        }
                        else
                        {
                            foreach (var item in neighbors)
                            {
                                queue.Enqueue(item);
                            }
                        }

                    }
                }
                if (cycled && atomNode.Parent != null)
                {
                    var cycle = new Cycle();
                    do
                    {
                        var from = atomNode.Value;
                        var toAtoms = _linkService.GetLinksFromAtom(atomNode.Value, links)
                            .Where(x => x.To.AtomId == atomNode.Parent.Value.AtomId).ToList();
                        cycle.Links.AddRange(toAtoms);

                        atomNode = atomNode.Parent;
                    } while (atomNode.Parent != null);

                    cycle.SetDiferentAtoms();
                    cycles.Add(cycle);
                }
            }

            return new List<Cycle>();
        }
    }
}
