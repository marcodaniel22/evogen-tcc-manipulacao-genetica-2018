using System.Collections.Generic;

namespace EvoGen.Helper
{
    public class Constants
    {
        public static readonly int MaxLinkEdges = 3;
        public static readonly int MaxMoleculeAtoms = 50;
        public static readonly List<string> ElementFilterList = new List<string> { "C", "H", "O", "N", "P", "S", "Br", "Cl", "I" };
        public static readonly Dictionary<string, int> OoctetRule = new Dictionary<string, int> {
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
        public static readonly List<string> CyclicCompoundAtoms = new List<string> { "C", "O", "N", "P", "S" };
    }
}
