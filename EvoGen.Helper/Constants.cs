using System;
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
        public static readonly Dictionary<Tuple<string, int, string>, int> EnergyTable = new Dictionary<Tuple<string, int, string>, int> {
            { new Tuple<string, int, string>("C", 1, "H"), 412 },
            { new Tuple<string, int, string>("C", 1, "H"), 412 },
            { new Tuple<string, int, string>("C", 1, "C"), 348 },
            { new Tuple<string, int, string>("C", 2, "C"), 612 },
            { new Tuple<string, int, string>("C", 3, "C"), 837 },
            { new Tuple<string, int, string>("C", 1, "O"), 360 },
            { new Tuple<string, int, string>("C", 2, "O"), 743 },
            { new Tuple<string, int, string>("C", 1, "N"), 305 },
            { new Tuple<string, int, string>("C", 2, "N"), 615 },
            { new Tuple<string, int, string>("C", 3, "N"), 891 },
            { new Tuple<string, int, string>("C", 1, "Cl"), 338 },
            { new Tuple<string, int, string>("C", 1, "Br"), 276 },
            { new Tuple<string, int, string>("C", 1, "I"), 238 },
            { new Tuple<string, int, string>("C", 1, "P"), 265 },
            { new Tuple<string, int, string>("C", 1, "S"), 260 },
            { new Tuple<string, int, string>("N", 1, "H"), 388 },
            { new Tuple<string, int, string>("N", 1, "N"), 163 },
            { new Tuple<string, int, string>("N", 2, "N"), 409 },
            { new Tuple<string, int, string>("N", 3, "N"), 944 },
            { new Tuple<string, int, string>("N", 1, "O"), 210 },
            { new Tuple<string, int, string>("N", 2, "O"), 630 },
            { new Tuple<string, int, string>("N", 1, "Cl"), 381 },
            { new Tuple<string, int, string>("N", 1, "Br"), 245 },
            { new Tuple<string, int, string>("N", 1, "I"), 159 },
            { new Tuple<string, int, string>("N", 1, "P"), 210 },
            { new Tuple<string, int, string>("N", 1, "S"), 0  },
            { new Tuple<string, int, string>("O", 1, "H"), 463 },
            { new Tuple<string, int, string>("O", 1, "O"), 157 },
            { new Tuple<string, int, string>("O", 2, "O"), 498 },
            { new Tuple<string, int, string>("O", 1, "Cl"), 205 },
            { new Tuple<string, int, string>("O", 1, "Br"), 210 },
            { new Tuple<string, int, string>("O", 1, "I"), 200 },
            { new Tuple<string, int, string>("O", 1, "P"), 350 },
            { new Tuple<string, int, string>("O", 1, "S"), 265 },
            { new Tuple<string, int, string>("H", 1, "H"), 436 },
            { new Tuple<string, int, string>("H", 1, "Cl"), 431 },
            { new Tuple<string, int, string>("H", 1, "Br"), 365 },
            { new Tuple<string, int, string>("H", 1, "I"), 298 },
            { new Tuple<string, int, string>("H", 1, "P"), 320 },
            { new Tuple<string, int, string>("H", 1, "S"), 340 },
            { new Tuple<string, int, string>("Cl", 1, "Cl"), 242 },
            { new Tuple<string, int, string>("Cl", 1, "Br"), 220 },
            { new Tuple<string, int, string>("Cl", 1, "l"), 210 },
            { new Tuple<string, int, string>("Cl", 1, "P"), 320 },
            { new Tuple<string, int, string>("Cl", 1, "S"), 250 },
            { new Tuple<string, int, string>("Br", 1, "Br"), 193 },
            { new Tuple<string, int, string>("Br", 1, "I"), 180 },
            { new Tuple<string, int, string>("Br", 1, "P"), 270 },
            { new Tuple<string, int, string>("Br", 1, "S"), 215 },
            { new Tuple<string, int, string>("I", 1, "I"), 151 },
            { new Tuple<string, int, string>("I", 1, "P"), 215 },
            { new Tuple<string, int, string>("I", 1, "S"), 170 },
            { new Tuple<string, int, string>("P", 1, "P"), 215 },
            { new Tuple<string, int, string>("S", 1, "S"), 215 }
        };
    }
}
