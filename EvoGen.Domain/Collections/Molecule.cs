using EvoGen.Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace EvoGen.Domain.Collections
{
    public class Molecule : MongoDbBase
    {
        public int AtomsCount { get; set; }
        public int DiferentAtomsCount { get; set; }
        public string SimpleAtoms { get; set; }
        public string SimpleLinks { get; set; }
        public string Nomenclature { get; set; }
        public string IdStructure { get; set; }
        public int Energy { get; set; }
        public bool FromDataSet { get; set; }

        [BsonIgnore]
        public List<Atom> Atoms { get; set; }
        [BsonIgnore]
        public List<Link> Links { get; set; }
    }
}
