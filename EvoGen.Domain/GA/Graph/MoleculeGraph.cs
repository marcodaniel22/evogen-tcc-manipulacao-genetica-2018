using EvoGen.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EvoGen.Domain.Collections
{
    public class MoleculeGraph
    {
        public List<AtomNode> AtomNodes { get; private set; }
        public List<LinkEdge> LinkEdges { get; private set; }
        public string Nomenclature { get; private set; }
        public string IdStructure { get; set; }

        private Random _random;

        public static List<AtomNode> ExtractAtomsFromNomenclature(string nomenclature)
        {
            Guard.StringNullOrEmpty(nomenclature, "Nomenclature");
            var atomNodes = new List<AtomNode>();

            var regexAtoms = new Regex(@"[A-Z][a-z]{0,1}[0-9]*");
            var regexAtomName = new Regex(@"[A-Z][a-z]{0,1}");
            var atomId = 1;
            foreach (var match in regexAtoms.Matches(nomenclature))
            {
                var atom = regexAtomName.Match(match.ToString()).Value;
                var count = Int32.Parse(string.IsNullOrEmpty(match.ToString().Replace(atom, "")) ? "1" : match.ToString().Replace(atom, ""));
                for (int i = 0; i < count; i++)
                    atomNodes.Add(new AtomNode(atom, atomId++));
            }
            return atomNodes;
        }

        public static string GetIdStructure(List<LinkEdge> linkEdges)
        {
            var groupByFrom = linkEdges.GroupBy(x => x.From.Symbol)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.From.Symbol).ToList());
            var groupByTo = linkEdges.Select(x => new LinkEdge(x.To, x.From)).GroupBy(x => x.From.Symbol)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.From.Symbol).ToList());
            var union = groupByFrom.Union(groupByTo).GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.ToList());

            var id = string.Empty;
            var linksCount = new Dictionary<string, int>();
            foreach (var symbolGroups in union)
            {
                foreach (var symbolList in symbolGroups.Value)
                {
                    foreach (var link in symbolList.Value)
                    {
                        var linkPair = link.From.Symbol + link.To.Symbol;
                        if (!linksCount.ContainsKey(linkPair))
                            linksCount.Add(linkPair, 1);
                        else
                        {
                            var count = linksCount[linkPair];
                            linksCount[linkPair] = count + 1;
                        }
                    }
                }
            }
            foreach (var linkPair in linksCount.OrderBy(x => x.Key))
            {
                id += linkPair.Key + linkPair.Value + "-";
            }
            return id.Substring(0, id.Length - 1);
        }

        public MoleculeGraph(string nomenclature, List<AtomNode> atoms)
        {
            this.Nomenclature = nomenclature;
            this.AtomNodes = atoms;
            this.LinkEdges = new List<LinkEdge>();
            this.GenerateRandomLinks();
        }

        public MoleculeGraph(string nomenclature, List<AtomNode> atoms, List<LinkEdge> links)
        {
            this.Nomenclature = nomenclature;
            this.AtomNodes = atoms;
            this.LinkEdges = links;
        }

        public int InterconectedAtoms()
        {
            var stack = new Stack<AtomNode>();
            var visited = new List<int>();
            var atomsCounter = 0;
            var first = this.LinkEdges[0].From;
            stack.Push(first);
            visited.Add(first.AtomId);
            while (stack.Count > 0)
            {
                atomsCounter++;
                var atom = stack.Pop();
                var links = this.GetLinksFromAtom(atom);
                foreach (var link in links)
                {
                    if (!visited.Contains(link.To.AtomId))
                    {
                        visited.Add(link.To.AtomId);
                        stack.Push(link.To);
                    }
                }
            }
            return atomsCounter;
        }

        private void GenerateRandomLinks()
        {
            _random = new Random(DateTime.Now.Millisecond);
            foreach (AtomNode fromAtom in this.AtomNodes)
            {
                AtomNode toAtom = null;
                do
                {
                    toAtom = this.AtomNodes[_random.Next(this.AtomNodes.Count)];
                } while (!this.NewLink(fromAtom, toAtom));
            }
        }

        public bool NewLink(AtomNode from, AtomNode to)
        {
            bool link = from.AtomId != to.AtomId;
            if (link) this.LinkEdges.Add(new LinkEdge(from, to));
            return link;
        }

        public List<LinkEdge> GetLinksFromAtom(AtomNode atom)
        {
            var links = this.LinkEdges.Where(x => x.From.AtomId == atom.AtomId).ToList();
            var outLinks = this.LinkEdges.Where(x => x.To.AtomId == atom.AtomId);
            foreach (var link in outLinks)
                links.Add(new LinkEdge(link.To, link.From));
            return links;
        }
    }
}
