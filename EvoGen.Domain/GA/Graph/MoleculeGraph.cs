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
                var links = this.GetAllLinksFromAtom(atom);
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
            if (this.AtomNodes != null)
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
        }

        public bool NewLink(AtomNode from, AtomNode to)
        {
            bool link = from.AtomId != to.AtomId;
            if (link) this.LinkEdges.Add(new LinkEdge(from, to));
            return link;
        }

        public List<LinkEdge> GetAllLinksFromAtom(AtomNode atom)
        {
            var links = this.LinkEdges.Where(x => x.From.AtomId == atom.AtomId).ToList();
            var outLinks = this.LinkEdges.Where(x => x.To.AtomId == atom.AtomId);
            foreach (var link in outLinks)
                links.Add(new LinkEdge(link.To, link.From));
            return links;
        }

        public void ReorganizeLinks()
        {
            var newLinks = new List<LinkEdge>();
            var queue = new Queue<AtomNode>();
            var stack = new Stack<LinkEdge>(this.LinkEdges);
            var stackedLinks = new List<string>();
            AtomNode lastAtom = null;

            stack.Push(this.LinkEdges.FirstOrDefault());
            while (stack.Count > 0)
            {
                var link = stack.Pop();
                if (!stackedLinks.Contains(link.ToString()))
                {
                    if (lastAtom == null)
                    {
                        lastAtom = link.From;
                        queue.Enqueue(lastAtom);
                    }

                    var sameLinks = this.LinkEdges.Where(x =>
                            (x.From.AtomId == link.From.AtomId && x.To.AtomId == link.To.AtomId)
                            || (x.From.AtomId == link.To.AtomId && x.To.AtomId == link.From.AtomId)
                        ).ToList();
                    if (link.From.AtomId == lastAtom.AtomId)
                    {
                        foreach (var item in sameLinks)
                        {
                            newLinks.Add(new LinkEdge(link.From, link.To));
                            stackedLinks.Add(item.ToString());
                        }
                        lastAtom = link.To;
                    }
                    else if (link.To.AtomId == lastAtom.AtomId)
                    {
                        foreach (var item in sameLinks)
                        {
                            newLinks.Add(new LinkEdge(link.To, link.From));
                            stackedLinks.Add(item.ToString());
                        }
                        lastAtom = link.From;
                    }
                    else
                    {
                        lastAtom = queue.Dequeue();
                        queue.Enqueue(lastAtom);
                        stack.Push(link);
                        continue;
                    }

                    if (!queue.Any(x => x.AtomId == lastAtom.AtomId))
                        queue.Enqueue(lastAtom);

                    var toStack = this.LinkEdges.Where(x => x.From.AtomId == lastAtom.AtomId || x.To.AtomId == lastAtom.AtomId).ToList();
                    toStack.RemoveAll(x =>
                            (x.From.AtomId == link.From.AtomId && x.To.AtomId == link.To.AtomId)
                            || (x.From.AtomId == link.To.AtomId && x.To.AtomId == link.From.AtomId)
                        );
                    foreach (var item in toStack)
                        stack.Push(item);
                }
            }
            if (newLinks.Count == this.LinkEdges.Count)
                this.LinkEdges = newLinks;
            else
                throw new Exception("Erro ao reorganizar links.");
        }


    }
}
