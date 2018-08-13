using EvoGen.Domain.Collections;
using EvoGen.Domain.ValueObjects;
using System.Collections.Generic;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface ILinkService
    {
        Link GetCollectionFromEdge(LinkEdge link);
        List<Link> GetLinksFromAtom(Atom atom, List<Link> links);
    }
}
