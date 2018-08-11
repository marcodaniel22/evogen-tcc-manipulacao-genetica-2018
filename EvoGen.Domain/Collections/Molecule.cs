using EvoGen.Domain.Collections.ValueObjects;
using System;
using System.Collections.Generic;

namespace EvoGen.Domain.Collections
{
    public class Molecule : MongoDbBase
    {
        public int AtomsCount { get; set; }
        public List<Link> Links { get; set; }
        public string Nomenclature { get; set; }
        public string IdStructure { get; set; }
    }
}
