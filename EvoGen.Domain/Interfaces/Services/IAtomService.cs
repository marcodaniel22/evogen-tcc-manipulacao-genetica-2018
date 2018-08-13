using EvoGen.Domain.Collections;
using EvoGen.Domain.ValueObjects;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IAtomService
    {
        Atom GetCollectionFromNode(AtomNode atom);
    }
}
