using EvoGen.Helper;
using EvoGen.Repository.Collection;
using EvoGen.Repository.Interfaces.Factory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace EvoGen.Repository.Factory
{
    public class MoleculeFactory : IMoleculeFactory
    {

        private readonly AtomFactory _atomFactory;

        public MoleculeFactory()
        {
            this._atomFactory = new AtomFactory();
        }

        public Molecule getCollectionByJToken(JToken jtoken)
        {
            var Energy = getEnergy(jtoken);
            var Atoms = getAtoms(jtoken);
            var ShapeM = getShapeM(jtoken);
            var ControlId = getControlId(jtoken);
            return new Molecule(Energy, Atoms, ShapeM, true, ControlId);
        }

        private double getEnergy(JToken json)
        {
            Guard.JTokenValidation("En", json);
            return Convert.ToDouble(json["En"]);
        }

        private List<Atom> getAtoms(JToken json)
        {
            Guard.JTokenValidation("atoms", json);
            var atoms = new List<Atom>();
            foreach (var atom in json["atoms"])
            {
                atoms.Add(_atomFactory.getCollectionByJToken(atom));
            }
            return atoms;
        }

        private List<double> getShapeM(JToken json)
        {
            Guard.JTokenValidation("shapeM", json);
            return json["shapeM"].ToObject<List<double>>();
        }

        private long getControlId(JToken json)
        {
            Guard.JTokenValidation("id", json);
            return json["id"].ToObject<long>();
        }

        public JToken getJTokenByCollection(Molecule obj)
        {
            throw new NotImplementedException();
        }

        public JToken getJTokenByCollectionList(IEnumerable<Molecule> list)
        {
            throw new NotImplementedException();
        }
    }
}
