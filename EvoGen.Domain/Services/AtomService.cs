using EvoGen.Domain.Collections;
using EvoGen.Domain.Collections.ValueObjects;
using EvoGen.Domain.Interfaces.Services;

namespace EvoGen.Domain.Services
{
    public class AtomService : IAtomService
    {
        public Atom GetCollectionFromNode(AtomNode atom)
        {
            var collection = new Atom();
            collection.AtomId = atom.AtomId;
            collection.Octet = atom.Octet;
            collection.Symbol = atom.Symbol;
            return collection;
        }
    }
}
