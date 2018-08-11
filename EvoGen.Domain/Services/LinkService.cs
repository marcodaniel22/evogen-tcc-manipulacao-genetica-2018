using EvoGen.Domain.Collections;
using EvoGen.Domain.Collections.ValueObjects;
using EvoGen.Domain.Interfaces.Services;

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
    }
}
