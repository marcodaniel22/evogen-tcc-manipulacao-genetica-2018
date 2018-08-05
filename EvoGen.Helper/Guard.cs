using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvoGen.Helper
{
    public class Guard
    {
        public static void IntLowerThanZero(int value, string prop)
        {
            if (value <= 0)
                throw new Exception(String.Format("Property '{0}' can't be lower than zero!", prop));
        }

        public static void ElementFilter(string symbol)
        {
            if (!Util.ElementFilterList.Contains(symbol))
                throw new Exception(String.Format("Element '{0}' can't be used at this time!", symbol));
        }

        public static void ListNullOrEmpty<T>(List<T> list, string prop)
        {
            if (list == null || (list != null && list.Count == 0))
                throw new Exception(String.Format("Property '{0}' can't be null or empty!", prop));
        }

        internal static void PrevendEndLowerThanBegin(int begin, int end, string prop)
        {
            if (begin > end)
                throw new Exception(String.Format("Property '{0}' can't have EndValue lower than BeginValue!", prop));
        }

        public static void DoubleLowerThanZero(double value, string prop)
        {
            if (value <= 0)
                throw new Exception(String.Format("Property '{0}' can't be lower than zero!", prop));
        }

        public static void LongLowerThanZero(long value, string prop)
        {
            if (value <= 0)
                throw new Exception(String.Format("Property '{0}' can't be lower than zero!", prop));
        }

        public static void StringNullOrEmpty(string value, string prop)
        {
            if (string.IsNullOrEmpty(value))
                throw new Exception(String.Format("Property '{0}' can't be null or empty!", prop));
        }

        public static void JTokenValidation(string hash, JToken json)
        {
            if (json[hash] == null)
                throw new Exception(String.Format("The json don't have the hash '{0}'!", hash));
        }
    }
}
