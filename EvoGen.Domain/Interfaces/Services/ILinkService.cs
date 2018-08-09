using EvoGen.Domain.Collections;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface ILinkService
    {
        Link GetCollectionFromEdge(LinkEdge link);
    }
}
