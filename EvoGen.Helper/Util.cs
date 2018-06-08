using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvoGen.Helper
{
    public class Util
    {
        public static int MaxLinkEdges = 3;
        public static List<string> ElementFilterList = new List<string> { "C", "H", "O", "N", "P", "S", "Br", "Cl", "I" };
        public static Dictionary<string, int> OoctetRule = new Dictionary<string, int> {
            { "C", 4 },
            { "H", 1},
            { "O", 6},
            { "N", 5},
            { "P", 5},
            { "S", 6},
            { "Br", 7},
            { "Cl", 7},
            { "I", 7}
        };
    }
}
