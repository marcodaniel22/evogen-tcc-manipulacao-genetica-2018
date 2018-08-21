using EvoGen.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace EvoGen.Domain.Collections
{
    public class Molecule : MongoDbBase
    {
        public int AtomsCount { get; set; }
        public int DiferentAtomsCount { get; set; }
        public List<Link> Links { get; set; }
        public string Nomenclature { get; set; }
        public string IdStructure { get; set; }
        public int Energy { get; set; }
        public bool Searched { get; set; }
        public bool FromDataSet { get; set; }
    }
}
