public class LimitedOrderedListNode<T>
{
    public LimitedOrderedListNode<T> Previous { get; set; }
    public LimitedOrderedListNode<T> Next { get; set; }
    public T Item { get; set; }

    public LimitedOrderedListNode(T item)
    {
        Previous = null;
        Next = null;
        Item = item;
    }
}