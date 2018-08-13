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
    }
}
