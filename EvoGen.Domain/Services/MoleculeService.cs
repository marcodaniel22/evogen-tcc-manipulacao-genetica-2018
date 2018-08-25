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
        private readonly ILogRepository _logRepository;
        private readonly ILinkService _linkService;

        public MoleculeService(IMoleculeRepository moleculeRepository, ILogRepository logRepository, ILinkService linkService)
            : base(moleculeRepository)
        {
            this._moleculeRepository = moleculeRepository;
            this._logRepository = logRepository;
            this._linkService = linkService;
        }

        #region RepositoryAcess

        public int GetMoleculeCount()
        {
            return _moleculeRepository.GetAll().Count();
        }

        public List<Molecule> GetByNomenclature(string nomenclature)
        {
            return _moleculeRepository.GetAll().Where(x => x.Nomenclature == nomenclature).ToList();
        }

        public Molecule GetByIdStructure(string nomenclature, string idStructure)
        {
            return _moleculeRepository.GetAll()
                .FirstOrDefault(x => x.Nomenclature == nomenclature && x.IdStructure == idStructure);
        }

        public Molecule Create(MoleculeGraph molecule)
        {
            return _moleculeRepository.Create(GetCollectionFromGraph(molecule));
        }

        public Molecule Delete(Molecule molecule)
        {
            return _moleculeRepository.Delete(molecule.guidString);
        }

        public int GetNotEmptyMoleculeCount(string nomenclature)
        {
            return _moleculeRepository.GetAll().Count(x => x.Nomenclature == nomenclature && !string.IsNullOrEmpty(x.IdStructure));
        }

        public Molecule GetFirstEmpty()
        {
            if (_logRepository.GetAll().Count() > 0)
            {
                var randon = new Random();
                var emptyMolecules = _moleculeRepository.GetAll().Where(x => string.IsNullOrEmpty(x.IdStructure));
                //var minAtoms = emptyMolecules.Where(x => x.AtomsCount > 3 && x.AtomsCount < 20).Min(x => x.AtomsCount);
                var toSearch = emptyMolecules.Where(x => x.AtomsCount > 3 && x.AtomsCount < 20);
                var emptyCounter = toSearch.Count();
                if (emptyCounter > 0)
                {
                    var skipElements = randon.Next(emptyCounter);
                    return toSearch.Skip(skipElements).FirstOrDefault();
                }
                else
                {
                    var minSearches = _logRepository.GetAll().Min(x => x.SearchCounter);
                    var skipElements = randon.Next(_logRepository.GetAll().Count(x => x.SearchCounter == minSearches));
                    var nomenclature = _logRepository.GetAll().Skip(skipElements).FirstOrDefault().Nomenclature;
                    return _moleculeRepository.GetAll().Where(x => x.Nomenclature == nomenclature).FirstOrDefault();
                }
            }
            return _moleculeRepository.GetAll().First(x => string.IsNullOrEmpty(x.IdStructure));
        }

        #endregion

        #region CustomServices

        public Molecule GetCollectionFromGraph(MoleculeGraph molecule)
        {
            var collection = new Molecule();
            collection.Nomenclature = molecule.Nomenclature;
            collection.IdStructure = molecule.IdStructure;
            collection.Energy = molecule.Energy;
            collection.FromDataSet = molecule.FromDataSet;
            if (molecule.AtomNodes != null && molecule.AtomNodes.Count > 0)
            {
                collection.AtomsCount = molecule.AtomNodes.Count;
                collection.DiferentAtomsCount = molecule.AtomNodes.GroupBy(x => x.Symbol).Count();
                collection.Links = molecule.LinkEdges.Select(x => _linkService.GetCollectionFromEdge(x)).ToList();
            }
            return collection;

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
                    cycle.CycliId = _linkService.GetIdStructure(cycle.Links);
                    if (!cycles.Any(x => x.CycliId == cycle.CycliId))
                        cycles.Add(cycle);
                }
            }

            return cycles;
        }

        #endregion
    }
}
