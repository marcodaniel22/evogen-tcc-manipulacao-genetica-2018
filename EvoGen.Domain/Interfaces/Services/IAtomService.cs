using EvoGen.Domain.Collections;
using EvoGen.Domain.Collections.ValueObjects;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IAtomService
    {
        Atom GetCollectionFromNode(AtomNode atom);
    }
}
