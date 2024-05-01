using System;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects
{
    public class Node<T>(T data) : IEquatable<Node<T>>, ILinkedListNode<Node<T>>, IHasData<T> where T:class  
    {

        public Node<T> Status { get; set; }

        public Node<T>? Other { get; set; }

        public Node<T> Ev { get; set; }

        public Node<T>? Previous { get; set; }

        public Node<T>? Next { get; set; }

        public bool IsRoot { get; set; }
        public T Data { get; set; } = data;

        public Node(): this(null!)
        {
        }

        public Node(T data,Action<Node<T>> nInit): this(data)
        {
            nInit(this);
        }

        public void Remove()
        {
            if (Previous != null)
                Previous.Next = Next;
            if (Next != null)
                Next.Previous = Previous;
            Previous = null;
            Next = null;
        }


        public bool Equals(Node<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return false;
            //return Equals(Previous, other.Previous) 
            //    && Equals(Next, other.Next) 
            //    && IsRoot == other.IsRoot;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return false;
            //            if (obj.GetType() != GetType()) return false;
            //            return Equals((Node<T>)obj);
        }

        //public override int GetHashCode()
        //{
        //    unchecked
        //    {
        //        var hashCode = (Previous != null ? Previous.GetHashCode() : 0);
        //        hashCode = (hashCode * 397) ^ (Next != null ? Next.GetHashCode() : 0);
        //        hashCode = (hashCode * 397) ^ IsRoot.GetHashCode();
        //        return hashCode;
        //    }
        //}
    }
}