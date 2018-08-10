using EvoGen.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
