using System;
using System.Collections.Generic;

namespace EvoGen.Domain.ValueObjects
{
    public class Node
    {
        public Node Parent { get; set; }
        public dynamic Value { get; set; }
        public List<Node> Children { get; set; }

        public Node()
        {
            this.Children = new List<Node>();
        }
    }
}
