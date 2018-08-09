using EvoGen.Domain.Collections;
using EvoGen.Domain.Interfaces.Services;
using System;

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
