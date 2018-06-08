using EvoGen.Helper;
using EvoGen.Repository.Collection;
using EvoGen.Repository.Interfaces.Factory;
using EvoGen.Repository.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace EvoGen.Repository.Factory
{
    public class AtomFactory : IAtomFactory
    {
        public Atom getCollectionByJToken(JToken jtoken)
        {
            Guard.JTokenValidation("type", jtoken);
            Guard.JTokenValidation("xyz", jtoken);
            var positions = jtoken["xyz"].ToObject<List<double>>();
            var ElementSymbol = jtoken["type"].Value<string>();
            var X = positions[0];
            var Y = positions[1];
            var Z = positions[2];
            return new Atom(ElementSymbol, X, Y, Z);
        }

        public JToken getJTokenByCollection(Atom obj)
        {
            throw new NotImplementedException();
        }

        public JToken getJTokenByCollectionList(IEnumerable<Atom> list)
        {
            throw new NotImplementedException();
        }
    }
}
