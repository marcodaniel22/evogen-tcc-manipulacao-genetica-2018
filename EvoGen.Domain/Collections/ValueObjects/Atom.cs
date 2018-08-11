﻿using System;

namespace EvoGen.Domain.Collections.ValueObjects
{
    public class Atom
    {
        public string Symbol { get; set; }
        public int Octet { get; set; }
        public int AtomId { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.AtomId, this.Symbol);
        }
    }
}