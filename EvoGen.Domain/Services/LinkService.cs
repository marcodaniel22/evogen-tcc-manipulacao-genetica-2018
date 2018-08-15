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
            var outLinks = links.Where(x => x.To.AtomId == atom.AtomId);
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
    }
}
