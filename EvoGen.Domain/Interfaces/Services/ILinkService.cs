using EvoGen.Domain.Collections;
using EvoGen.Domain.Collections.ValueObjects;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface ILinkService
    {
        Link GetCollectionFromEdge(LinkEdge link);
    }
}
