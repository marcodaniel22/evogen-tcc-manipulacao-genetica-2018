using System;

namespace EvoGen.Domain.Collections
{
    public class Log : MongoDbBase
    {
        public string Nomenclature { get; set; }
        public int SearchCounter { get; set; }
    }
}
