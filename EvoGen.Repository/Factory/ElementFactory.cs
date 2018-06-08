using System.Collections.Generic;
using EvoGen.Helper;
using EvoGen.Repository.Collection;
using EvoGen.Repository.Interfaces.Factory;
using Newtonsoft.Json.Linq;

namespace EvoGen.Repository.Factory
{
    public class ElementFactory : IElementFactory
    {
        public Element getCollectionByJToken(JToken jtoken)
        {
            var name = getName(jtoken);
            var symbol = getSymbol(jtoken);
            //Converter double to int
            var atomicNumber = getAtomicNumber(jtoken);
            var period = getPeriod(jtoken);
            //el.Category = item["Category"];
            var atomicWeight = getAtomicWeight(jtoken);
            var valenceLayer = getValenceLayer(jtoken);
            return new Element(name, symbol, atomicNumber, period, atomicWeight, valenceLayer);
        }

        private string getName(JToken json)
        {
            Guard.JTokenValidation("Name", json);
            return json["Name"].Value<string>();
        }

        private string getSymbol(JToken json)
        {
            Guard.JTokenValidation("Symbol", json);
            return json["Symbol"].Value<string>();
        }

        private int getAtomicNumber(JToken json)
        {
            Guard.JTokenValidation("AtomicNumber", json);
            return json["AtomicNumber"].Value<int>();
        }

        private int getPeriod(JToken json)
        {
            Guard.JTokenValidation("Period", json);
            return json["Period"].Value<int>();
        }

        private double getAtomicWeight(JToken json)
        {
            Guard.JTokenValidation("AtomicWeight", json);
            return json["AtomicWeight"].Value<double>();
        }

        private int getValenceLayer(JToken json)
        {
            Guard.JTokenValidation("ValenceLayer", json);
            return json["ValenceLayer"].Value<int>();
        }

        public JToken getJTokenByCollection(Element obj)
        {
            throw new System.NotImplementedException();
        }

        public JToken getJTokenByCollectionList(IEnumerable<Element> list)
        {
            throw new System.NotImplementedException();
        }
    }
}
