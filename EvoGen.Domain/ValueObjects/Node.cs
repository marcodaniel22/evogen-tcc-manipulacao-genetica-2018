using EvoGen.Domain.Collections;
using System;
using System.Collections.Generic;

namespace EvoGen.Domain.ValueObjects
{
    public class Node<T>
    {
        public Node<T> Parent { get; set; }
        public T Value { get; set; }
        public List<Node<T>> Children { get; set; }

        public Node()
        {
            this.Children = new List<Node<T>>();
        }
    }
}
