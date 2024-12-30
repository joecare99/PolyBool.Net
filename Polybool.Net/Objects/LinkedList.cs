﻿using System;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects
{
    public class LinkedList<T, T2> where T : class, ILinkedListNode<T>, IHasData<T2>, new()
        where T2 : class, IComparable<T2>
    {
        public LinkedList()
        {
            Root = new T { IsRoot = true };
        }

        private T Root { get; }

        public bool Exists(T node)
        {
            if (node == null || Equals(node, Root))
                return false;
            return true;
        }

        public bool IsEmpty()
        {
            return Root.Next == null;
        }

        public T? GetHead()
        {
            return Root.Next;
        }

        public T2? First()
        {
            return Root.Next?.Data;
        }

        public void Insert(T2 data)
        {
            var node = new T { Data = data };
            var last = Root;
            var here = Root.Next;
            while (here != null && data.CompareTo(here.Data)>0)
            {
                last = here;
                here = here.Next;
            }
            if (here != null)
                here.Previous = node;
            node.Next = here;
            last.Next = node;
            node.Previous = last;
        }

        public void Insert(T2 data,Func<T2,bool> Check)
        {
            var node = new T { Data = data };
            var last = Root;
            var here = Root.Next;
            while (here != null && Check(here.Data) )
            {
                last = here;
                here = here.Next;
            }
            if (here != null)
                here.Previous = node;
            node.Next = here;
            last.Next = node;
            node.Previous = last;
        }

        public void Remove(T2 data)
        {
            var last = Root;
            var here = Root.Next;
            while (here != null && !ReferenceEquals(data,here.Data) )
            {
                last = here;
                here = here.Next;
            }
            if (here == null)
                return;
            last.Next = here.Next;
            if (here.Next != null)
                here.Next.Previous = last;
        }

        public void InsertBefore(T node, Func<T, bool> check)
        {
            var last = Root;
            var here = Root.Next;
            while (here != null)
            {
                if (check(here))
                {
                    node.Previous = here.Previous;
                    node.Next = here;
                    if (here.Previous != null)
                        here.Previous.Next = node;
                    here.Previous = node;
                    return;
                }
                last = here;
                here = here.Next;
            }
            last.Next = node;
            node.Previous = last;
            node.Next = null;
        }

        public Transition<T> FindTransition(Func<T, bool> check)
        {
            var previous = Root;
            var here = Root.Next;
            while (here != null)
            {
                if (check(here))
                    break;
                previous = here;
                here = here.Next;
            }
            return new Transition<T>
            {
                previous = previous,
                Before = Equals(previous, Root) ? null : previous,
                After = here
            };
        }
    }
}