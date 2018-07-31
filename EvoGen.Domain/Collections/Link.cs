using System;

namespace EvoGen.Domain.Collections
{
    public class Link : MongoDbBase
    {
        public Atom From { get; set; }
        public Atom To { get; set; }
        
        public override string ToString()
        {
            return String.Format("{0}-{1} => {2}-{3}", this.From.AtomId, this.From.Symbol, To.AtomId, this.To.Symbol);
        }
    }
}
