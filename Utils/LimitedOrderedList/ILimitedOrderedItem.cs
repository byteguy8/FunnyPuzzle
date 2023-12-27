public interface ILimitedOrderedItem<T>
{
    int Compare(T item);
}