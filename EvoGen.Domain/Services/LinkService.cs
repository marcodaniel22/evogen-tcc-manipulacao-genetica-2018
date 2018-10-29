using EvoGen.Domain.Collections;
using EvoGen.Domain.ValueObjects;
using EvoGen.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;

namespace EvoGen.Domain.Services
{
    public class LinkService : ILinkService
    {
        private readonly IAtomService _atomService;

        public LinkService(IAtomService atomService)
        {
            this._atomService = atomService;
        }

        public Link GetCollectionFromEdge(LinkEdge link)
        {
            var collection = new Link();
            collection.From = _atomService.GetCollectionFromNode(link.From);
            collection.To = _atomService.GetCollectionFromNode(link.To);
            return collection;
        }

        public List<Link> GetCollectionsFromEdges(List<LinkEdge> links)
        {
            var listResult = new List<Link>();
            foreach (var link in links)
            {
                listResult.Add(new Link(new Atom(link.From), new Atom(link.To)));
            }
            return listResult;
        }

        public List<Link> GetLinksFromAtom(Atom atom, List<Link> links)
        {
            var atomLinks = links.Where(x => x.From.AtomId == atom.AtomId).ToList();
            var outLinks = links.Where(x => x.To.AtomId == atom.AtomId).ToList();
            foreach (var link in outLinks)
                atomLinks.Add(new Link()
                {
                    From = link.To,
                    To = link.From
                });
            return atomLinks;
        }

        public string GetIdStructure(List<LinkEdge> linkEdges)
        {
            return GetIdStructure(GetCollectionsFromEdges(linkEdges));
        }

        public string GetIdStructure(List<Link> linkEdges)
        {
            var groupByFrom = linkEdges.GroupBy(x => x.From.Symbol)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.From.Symbol).ToList());
            var groupByTo = linkEdges.Select(x => new Link(x.To, x.From)).GroupBy(x => x.From.Symbol)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.From.Symbol).ToList());
            var union = groupByFrom.Union(groupByTo).GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.ToList());

            var id = string.Empty;
            var linksCount = new Dictionary<string, int>();
            foreach (var symbolGroups in union)
            {
                foreach (var symbolList in symbolGroups.Value)
                {
                    foreach (var link in symbolList.Value)
                    {
                        var linkPair = link.From.Symbol + link.To.Symbol;
                        if (!linksCount.ContainsKey(linkPair))
                            linksCount.Add(linkPair, 1);
                        else
                        {
                            var count = linksCount[linkPair];
                            linksCount[linkPair] = count + 1;
                        }
                    }
                }
            }
            foreach (var linkPair in linksCount.OrderBy(x => x.Key))
            {
                id += linkPair.Key + linkPair.Value + "-";
            }
            return id.Substring(0, id.Length - 1);
        }

        public int CountDiferentLinks(List<Link> linkEdges)
        {
            var auxList = new List<Link>();
            foreach (var link in linkEdges)
            {
                if (!auxList.Any(x => x.From.Symbol == link.From.Symbol && x.To.Symbol == link.To.Symbol
                    || x.From.Symbol == link.To.Symbol && x.To.Symbol == link.From.Symbol))
                {
                    auxList.Add(link);
                }
            }
            return auxList.Count;
        }

        public List<Atom> GetDiferentAtomsFromLinks(List<Link> linkEdges)
        {
            var atomList = new List<Atom>();
            foreach (var link in linkEdges)
            {
                if (!atomList.Contains(link.From))
                    atomList.Add(link.From);
                if (!atomList.Contains(link.To))
                    atomList.Add(link.To);
            }
            return atomList;
        }

        public List<Link> GetOutCycleLinks(Molecule molecule, List<Cycle> cycles)
        {
            var outLinks = molecule.Links.ToList();
            foreach (var cycle in cycles)
            {
                foreach (var link in cycle.Links)
                {
                    if (outLinks.Any(x => x.From.AtomId == link.From.AtomId && x.To.AtomId == link.To.AtomId
                        || x.From.AtomId == link.To.AtomId && x.To.AtomId == link.From.AtomId))
                    {
                        outLinks.RemoveAll(x => x.From.AtomId == link.From.AtomId && x.To.AtomId == link.To.AtomId
                            || x.From.AtomId == link.To.AtomId && x.To.AtomId == link.From.AtomId);
                    }
                }
            }
            return outLinks;
        }
    }
}
