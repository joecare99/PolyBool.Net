namespace PolyBool.Net.Interfaces
{
    public interface ILinkedListNode<T> where T : ILinkedListNode<T>
    {
        T? Next { get; set; }
        T? Previous { get; set; }
        bool IsRoot { get; set; }
    }
}