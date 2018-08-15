using System;
using System.Collections.Generic;

namespace EvoGen.Domain.ValueObjects
{
    public class Cycle
    {
        public List<Link> Links { get; private set; }
        public int DiferentAtoms { get; private set; }
        public string CycliId { get; set; }

        public Cycle()
        {
            this.Links = new List<Link>();
        }

        public void SetDiferentAtoms()
        {
            var atomIds = new List<int>();
            foreach (var link in this.Links)
            {
                if (!atomIds.Contains(link.From.AtomId))
                    atomIds.Add(link.From.AtomId);
                if (!atomIds.Contains(link.To.AtomId))
                    atomIds.Add(link.To.AtomId);
            }
            this.DiferentAtoms = atomIds.Count;
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}", this.DiferentAtoms, this.CycliId);
        }
    }
}
