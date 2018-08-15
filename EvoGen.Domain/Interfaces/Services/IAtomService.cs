using EvoGen.Domain.Collections;
using EvoGen.Domain.ValueObjects;
using System.Collections.Generic;

namespace EvoGen.Domain.Interfaces.Services
{
    public interface IAtomService
    {
        Atom GetCollectionFromNode(AtomNode atom);
    }
}
