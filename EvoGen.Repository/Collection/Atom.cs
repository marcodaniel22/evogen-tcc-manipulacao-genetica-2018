using EvoGen.Helper;
using System;

namespace EvoGen.Repository.Collection
{
    public class Atom : MongoDbBase
    {
        public string ElementSymbol { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        protected Atom() { }

        public Atom(string elementSymbol, double x, double y, double z)
        {
            setElementSymbol(elementSymbol);
            setX(x);
            setY(y);
            setZ(z);
            setGuidString();
        }

        public void setElementSymbol(string elementSymbol)
        {
            Guard.StringNullOrEmpty(elementSymbol, "ElementSymbol");
            Guard.ElementFilter(elementSymbol);
            this.ElementSymbol = elementSymbol;
        }

        private void setX(double x)
        {
            this.X = x;
        }

        private void setY(double y)
        {
            this.Y = y;
        }

        private void setZ(double z)
        {
            this.Z = z;
        }

        public void setGuidString()
        {
            var guid = Guid.NewGuid();
            this.guidString = guid.ToString();
        }
    }
}
