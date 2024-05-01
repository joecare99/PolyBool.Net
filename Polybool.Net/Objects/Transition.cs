using System;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects
{
    public class Transition<T> where T : class, ILinkedListNode<T>
    {
        internal T previous { private get; set; }
        public T? After { get; set; }
        public T? Before { get; set; }

        public T Insert(T node) 
        {
            node.Previous = previous;
            node.Next = After;
            previous.Next = node;
            if (After != null)
                After.Previous = node;
            return node;
            
        }
    }
}