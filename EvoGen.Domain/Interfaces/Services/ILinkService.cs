using EvoGen.Domain.Collections;
using EvoGen.Domain.ValueObjects;
using System.Collections.Generic;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface ILinkService
    {
        Link GetCollectionFromEdge(LinkEdge link);
        List<Link> GetCollectionsFromEdges(List<LinkEdge> links);
        List<Link> GetLinksFromAtom(Atom atom, List<Link> links);
        string GetIdStructure(List<LinkEdge> linkEdges);
        string GetIdStructure(List<Link> linkEdges);
        int CountDiferentLinks(List<Link> linkEdges);
        List<Atom> GetDiferentAtomsFromLinks(List<Link> linkEdges);
    }
}
