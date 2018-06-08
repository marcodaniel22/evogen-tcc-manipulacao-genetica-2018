using EvoGen.Helper;
using System;

namespace EvoGen.Repository.Collection
{
    public class Element : MongoDbBase
    {
        public string Name { get; private set; }
        public string Symbol { get; private set; }
        public int AtomicNumber { get; private set; }
        public int Period { get; private set; }
        public Enums.Category Category { get; private set; }
        public double AtomicWeight { get; private set; }
        public int ValenceLayer { get; private set; }

        protected Element() { }

        public Element(string name, string symbol, int atomicNumber, int period, double atomicWeight, int valenceLayer)
        {
            setName(name);
            setSymbol(symbol);
            setAtomicNumber(atomicNumber);
            setPeriod(period);
            setAtomicWeight(atomicWeight);
            setValenceLayer(valenceLayer);
            setGuidString();
        }

        private void setName(string name)
        {
            Guard.StringNullOrEmpty(name, "Name");
            this.Name = name;
        }

        private void setSymbol(string symbol)
        {
            Guard.StringNullOrEmpty(symbol, "Symbol");
            Guard.ElementFilter(symbol);
            this.Symbol = symbol;
        }

        private void setAtomicNumber(int atomicNumber)
        {
            Guard.LongLowerThanZero(atomicNumber, "AtomicNumber");
            this.AtomicNumber = atomicNumber;
        }

        private void setPeriod(int period)
        {
            Guard.LongLowerThanZero(period, "Period");
            this.Period = period;
        }

        private void setAtomicWeight(double atomicWeight)
        {
            Guard.DoubleLowerThanZero(atomicWeight, "AtomicWeight");
            this.AtomicWeight = atomicWeight;
        }

        private void setValenceLayer(int valenceLayer)
        {
            Guard.LongLowerThanZero(valenceLayer, "ValenceLayer");
            this.ValenceLayer = valenceLayer;
        }

        public void setGuidString()
        {
            var guid = Guid.NewGuid();
            this.guidString = guid.ToString();
        }
    }
}
