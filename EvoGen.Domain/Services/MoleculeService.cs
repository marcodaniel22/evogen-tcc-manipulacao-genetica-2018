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
        private readonly IAtomService _atomService;

        public MoleculeService(IMoleculeRepository moleculeRepository, ILogRepository logRepository, ILinkService linkService, IAtomService atomService)
            : base(moleculeRepository)
        {
            this._moleculeRepository = moleculeRepository;
            this._logRepository = logRepository;
            this._linkService = linkService;
            this._atomService = atomService;
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

        public Molecule GetRandomEmpty(int min, int max)
        {
            var randon = new Random();
            var empty = _moleculeRepository.GetAll().Where(x => string.IsNullOrEmpty(x.IdStructure));
            var query = empty.Where(x => x.AtomsCount >= min && x.AtomsCount <= max);

            var counter = query.Count();
            if (counter > 0)
            {
                var skip = randon.Next(counter);
                return query.Skip(skip).FirstOrDefault();
            }
            else
            {
                var minLog = _logRepository.GetAll().Min(x => x.SearchCounter);
                var skip = randon.Next(_logRepository.GetAll().Count(x => x.SearchCounter == minLog));
                var nomenclature = _logRepository.GetAll().Skip(skip).FirstOrDefault().Nomenclature;
                return _moleculeRepository.GetAll().Where(x => x.Nomenclature == nomenclature).FirstOrDefault();
            }
        }

        public Molecule GetRandomToSearch(int min, int max)
        {
            var randon = new Random();
            var query = _moleculeRepository.GetAll().Where(x => x.AtomsCount >= min && x.AtomsCount <= max);

            var minLog = _logRepository.GetAll().Min(x => x.SearchCounter);
            var skip = randon.Next(_logRepository.GetAll().Count(x => x.SearchCounter == minLog));
            var nomenclature = _logRepository.GetAll().Skip(skip).FirstOrDefault().Nomenclature;
            return query.Where(x => x.Nomenclature == nomenclature).FirstOrDefault();
        }

        public List<Molecule> GetMoleculesByRange(int init, int last)
        {
            return _moleculeRepository.GetAll().Where(x => !string.IsNullOrEmpty(x.IdStructure) && x.AtomsCount >= init && x.AtomsCount < last).ToList();
        }

        #endregion

        #region CustomServices

        public Molecule GetCollectionFromGraph(MoleculeGraph molecule)
        {
            var collection = new Molecule();
            if (molecule.AtomNodes != null && molecule.AtomNodes.Count > 0)
            {
                collection.AtomsCount = molecule.AtomNodes.Count;
                collection.DiferentAtomsCount = molecule.AtomNodes.GroupBy(x => x.Symbol).Count();
                collection.SimpleAtoms = string.Join(",", molecule.AtomNodes.Select(x => x.ToString()).ToList());
                collection.Atoms = molecule.AtomNodes.Select(x => _atomService.GetCollectionFromNode(x)).ToList();
            }
            if (molecule.LinkEdges != null && molecule.LinkEdges.Count > 0)
            {
                collection.SimpleLinks = string.Join(",", molecule.LinkEdges.Select(x => _linkService.GetCollectionFromEdge(x)).Select(x => x.ToString()).ToList());
                collection.Links = molecule.LinkEdges.Select(x => _linkService.GetCollectionFromEdge(x)).ToList();
            }
            collection.Nomenclature = molecule.Nomenclature;
            collection.IdStructure = molecule.IdStructure;
            collection.Energy = molecule.Energy;
            collection.FromDataSet = molecule.FromDataSet;
            return collection;
        }

        public List<Cycle> GetMoleculeCycles(Molecule molecule)
        {
            BuildDatabaseMolecule(ref molecule);
            List<Cycle> cycles = new List<Cycle>();
            var links = molecule.Links.Where(x => Constants.CyclicCompoundAtoms.Contains(x.From.Symbol) && Constants.CyclicCompoundAtoms.Contains(x.To.Symbol)).ToList();
            var atoms = _linkService.GetDiferentAtomsFromLinks(links);
            foreach (var startAtom in atoms)
            {
                var cycled = false;
                var queue = new Queue<Node<Atom>>();
                var startNode = new Node<Atom>();
                startNode.Parent = null;
                startNode.Value = startAtom;
                queue.Enqueue(startNode);

                while (queue.Count > 0 && !cycled)
                {
                    var atomNode = queue.Dequeue();
                    var neighbors = _linkService.GetLinksFromAtom(atomNode.Value, links);
                    foreach (var neighbor in neighbors)
                    {
                        var neighborNode = new Node<Atom>();
                        neighborNode.Parent = atomNode;
                        neighborNode.Value = neighbor.To;

                        if (neighborNode.Value.AtomId == startAtom.AtomId && CountLevels(neighborNode) > 2)
                        {
                            cycled = true;
                            startNode = neighborNode;
                            break;
                        }

                        if (!AtomInParents(neighborNode))
                            queue.Enqueue(neighborNode);
                    }
                }

                if (cycled)
                {
                    var cycleLinks = new List<Link>();
                    var atom = startNode.Value;
                    var parent = startNode.Parent;
                    while (parent != null)
                    {
                        var from = atom;
                        var to = parent.Value;

                        foreach (var link in links)
                        {
                            if (from.AtomId == link.From.AtomId && to.AtomId == link.To.AtomId || from.AtomId == link.To.AtomId && to.AtomId == link.From.AtomId)
                                cycleLinks.Add(new Link(from, to));
                        }
                        atom = parent.Value;
                        parent = parent.Parent;
                    }
                    var cycleId = _linkService.GetIdStructure(cycleLinks);
                    if (!cycles.Any(x => x.CycliId == cycleId))
                        cycles.Add(new Cycle(cycleLinks, cycleId));
                }
            }

            return cycles;
        }

        private int CountLevels(Node<Atom> atomNode)
        {
            var parent = atomNode.Parent;
            var counter = 0;
            while (parent != null)
            {
                parent = parent.Parent;
                counter++;
            }
            return counter;
        }

        private bool AtomInParents(Node<Atom> atomNode)
        {
            var target = atomNode.Value;
            var parent = atomNode.Parent;
            while (parent != null && parent.Value.AtomId != target.AtomId)
                parent = parent.Parent;

            return parent != null;
        }

        public void BuildDatabaseMolecule(ref Molecule molecule)
        {
            if (molecule.Atoms == null)
            {
                var atoms = molecule.SimpleAtoms.Split(',');
                molecule.Atoms = atoms.Select(atom =>
                {
                    var atomProps = atom.Split('-');
                    return new Atom()
                    {
                        AtomId = Int32.Parse(atomProps[0]),
                        Symbol = atomProps[1],
                        Octet = Constants.OoctetRule[atomProps[1]]
                    };
                }).ToList();
            }
            if (molecule.Links == null)
            {
                var links = molecule.SimpleLinks.Split(',');
                molecule.Links = links.Select(link =>
                {
                    var linkAtoms = link.Replace("=", "").Split('>');
                    var from = linkAtoms[0].Split('-');
                    var to = linkAtoms[1].Split('-');
                    return new Link()
                    {
                        From = new Atom()
                        {
                            AtomId = Int32.Parse(from[0]),
                            Symbol = from[1],
                            Octet = Constants.OoctetRule[from[1]]
                        },
                        To = new Atom()
                        {
                            AtomId = Int32.Parse(to[0]),
                            Symbol = to[1],
                            Octet = Constants.OoctetRule[to[1]]
                        }
                    };
                }).ToList();
            }
        }

        public Molecule CloneMolecule(Molecule molecule)
        {
            BuildDatabaseMolecule(ref molecule);
            var newMolecule = new Molecule();
            newMolecule.Nomenclature = molecule.Nomenclature;
            newMolecule.AtomsCount = molecule.AtomsCount;
            newMolecule.DiferentAtomsCount = molecule.DiferentAtomsCount;
            newMolecule.Atoms = new List<Atom>();
            foreach (var atom in molecule.Atoms)
            {
                newMolecule.Atoms.Add(new Atom
                {
                    AtomId = atom.AtomId,
                    Octet = atom.Octet,
                    Symbol = atom.Symbol
                });
            }
            newMolecule.Links = new List<Link>();
            foreach (var link in molecule.Links)
            {
                newMolecule.Links.Add(new Link
                {
                    From = newMolecule.Atoms.FirstOrDefault(x => x.AtomId == link.From.AtomId),
                    To = newMolecule.Atoms.FirstOrDefault(x => x.AtomId == link.To.AtomId)
                });
            }
            return newMolecule;
        }

        #endregion
    }
}
