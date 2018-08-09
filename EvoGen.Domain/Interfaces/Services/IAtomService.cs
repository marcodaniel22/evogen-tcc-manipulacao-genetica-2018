using EvoGen.Domain.Collections;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IAtomService
    {
        Atom GetCollectionFromNode(AtomNode atom);
    }
}
