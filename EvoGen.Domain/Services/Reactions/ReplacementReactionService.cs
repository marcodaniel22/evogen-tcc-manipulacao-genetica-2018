using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Domain.Interfaces.Services.Reaction;
using EvoGen.Domain.ValueObjects;
using EvoGen.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvoGen.Domain.Services.Reactions
{
    public class ReplacementReactionService : IReplacementReactionService
    {
        private readonly IMoleculeService _moleculeService;
        private readonly ILinkService _linkService;

        public ReplacementReactionService(IMoleculeService moleculeService, ILinkService linkService)
        {
            this._moleculeService = moleculeService;
            this._linkService = linkService;
        }

        public Molecule React(Molecule reagent, Molecule substractMolecule)
        {
            var substract = _moleculeService.CloneMolecule(substractMolecule);
            // Get cycles
            var reagentCycles = _moleculeService.GetMoleculeCycles(reagent);
            var substractCycles = _moleculeService.GetMoleculeCycles(substract);

            if (substractCycles.Count >= 0 && substractCycles.Count <= 2)
            {
                // Get links out cycles
                var substractOutCycleLinks = _linkService.GetOutCycleLinks(substract, substractCycles);
                var reagentOutCycleLinks = _linkService.GetOutCycleLinks(reagent, reagentCycles);

                // Get links for replacement reaction
                var substractReactionLinks = GetSubstractLinks(substractOutCycleLinks);
                var reagentReactionLinks = GetReagentLinks(reagentOutCycleLinks);

                if (substractReactionLinks.Count >= 1 && reagentReactionLinks.Count >= 1)
                {
                    // Get first link for reaction
                    var substractReactionLink = substractReactionLinks.FirstOrDefault();
                    var reagentReactionLink = reagentReactionLinks.FirstOrDefault();

                    // Select atom that will be used in replacement reaction
                    var substractTargetAtom = GetSubstractTargetAtom(substractReactionLink);
                    var reagentTargetAtom = GetReagentTargetAtom(reagentReactionLink);

                    // Select more eletro positive atom of reagent neighbors for replacement
                    var reagentTargetAtomNeighbors = _linkService.GetLinksFromAtom(reagentTargetAtom, reagent.Links);
                    var positiveLink = GetMoreEletroPositiveLink(reagentTargetAtomNeighbors);

                    // Get rest of links to exclude for substract
                    var linksToExclude = GetTreeLinksToExclude(reagentTargetAtom, positiveLink, reagent);
                    var newLinks = reagent.Links.ToList();
                    foreach (var exclude in linksToExclude)
                    {
                        newLinks.RemoveAll(x => x.From.AtomId == exclude.From.AtomId && x.To.AtomId == exclude.To.AtomId
                            || x.From.AtomId == exclude.To.AtomId && x.To.AtomId == exclude.From.AtomId);
                    }

                    var targetLink = substract.Links.Where(x => x.From.AtomId == substractReactionLink.From.AtomId && x.To.AtomId == substractReactionLink.To.AtomId
                            || x.From.AtomId == substractReactionLink.To.AtomId && x.To.AtomId == substractReactionLink.From.AtomId).FirstOrDefault();

                    var targetAtomId = targetLink.From.AtomId;
                    if (substractReactionLink.To.AtomId == targetLink.To.AtomId)
                        targetAtomId = targetLink.To.AtomId;
                    var newAtom = new Atom()
                    {
                        AtomId = targetAtomId,
                        Symbol = reagentTargetAtom.Symbol,
                        Octet = reagentTargetAtom.Octet
                    };
                    var newLink = new Link(targetLink.From, newAtom);

                    substract.Links.Remove(targetLink);
                    substract.Links.Add(newLink);
                    substract.Atoms.Remove(substractTargetAtom);
                    substract.Atoms.Add(newAtom);

                    var mapper = new Dictionary<int, int>();
                    mapper.Add(reagentTargetAtom.AtomId, substractTargetAtom.AtomId);
                    foreach (var link in newLinks)
                    {
                        if (!mapper.ContainsKey(link.From.AtomId))
                        {
                            var newAtomId = substract.Atoms.Count + 1;
                            mapper.Add(link.From.AtomId, newAtomId);
                            substract.Atoms.Add(new Atom()
                            {
                                AtomId = newAtomId,
                                Symbol = link.From.Symbol,
                                Octet = link.From.Octet
                            });
                        }
                        if (!mapper.ContainsKey(link.To.AtomId))
                        {
                            var newAtomId = substract.Atoms.Count + 1;
                            mapper.Add(link.To.AtomId, newAtomId);
                            substract.Atoms.Add(new Atom()
                            {
                                AtomId = newAtomId,
                                Symbol = link.To.Symbol,
                                Octet = link.To.Octet
                            });
                        }
                        substract.Links.Add(
                            new Link(
                                new Atom()
                                {
                                    AtomId = mapper[link.From.AtomId],
                                    Symbol = link.From.Symbol,
                                    Octet = link.From.Octet
                                },
                                new Atom()
                                {
                                    AtomId = mapper[link.To.AtomId],
                                    Symbol = link.To.Symbol,
                                    Octet = link.To.Octet
                                }
                            )
                        );
                    }
                }
            }
            return substract;
        }

        private List<Link> GetTreeLinksToExclude(Atom target, Link positiveLink, Molecule reagent)
        {
            var links = new List<Link>();
            links.Add(positiveLink);
            var queue = new Queue<Atom>();
            var visitedAtoms = new List<int>();
            var parent = positiveLink.From;

            queue.Enqueue(positiveLink.To);
            while (queue.Count > 0)
            {
                var atom = queue.Dequeue();
                if (!visitedAtoms.Contains(atom.AtomId))
                {
                    visitedAtoms.Add(atom.AtomId);
                    var neighbors = _linkService.GetLinksFromAtom(atom, reagent.Links).Where(x => x.To.AtomId != parent.AtomId).ToList();
                    foreach (var neighbor in neighbors)
                    {
                        links.Add(neighbor);
                        queue.Enqueue(neighbor.To);
                    }
                    parent = atom;
                }
            }
            return links;
        }

        private Link GetMoreEletroPositiveLink(List<Link> reagentTargetAtomNeighbors)
        {
            foreach (var atom in Constants.ElectroPositiveAtoms)
            {
                foreach (var link in reagentTargetAtomNeighbors)
                {
                    if (link.To.Symbol == atom)
                        return link;
                }
            }
            return null;
        }

        public List<Link> GetReagentLinks(List<Link> reagentOutCycleLinks)
        {
            foreach (var atom in Constants.FreeElectrons)
            {
                if (reagentOutCycleLinks.Any(x => x.From.Symbol == atom.Key || x.To.Symbol == atom.Key))
                    return reagentOutCycleLinks.Where(x => x.From.Symbol == atom.Key || x.To.Symbol == atom.Key).ToList();
            }
            return new List<Link>();
        }

        public List<Link> GetSubstractLinks(List<Link> substractOutCycleLinks)
        {
            return substractOutCycleLinks.Where(x => Constants.HalogensAtoms.Contains(x.From.Symbol) || Constants.HalogensAtoms.Contains(x.To.Symbol)).ToList();
        }

        public Atom GetSubstractTargetAtom(Link link)
        {
            if (Constants.HalogensAtoms.Contains(link.From.Symbol))
                return link.From;
            else if (Constants.HalogensAtoms.Contains(link.To.Symbol))
                return link.To;
            return null;
        }

        public Atom GetReagentTargetAtom(Link link)
        {
            foreach (var atom in Constants.FreeElectrons)
            {
                if (atom.Key == link.From.Symbol)
                    return link.From;
                else if (atom.Key == link.To.Symbol)
                    return link.To;
            }
            return null;
        }
    }
}
