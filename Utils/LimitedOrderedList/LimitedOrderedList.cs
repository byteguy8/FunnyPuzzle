using System;

public class LimitedOrderedList<T> where T : ILimitedOrderedItem<T>
{
	public int Size;
	public int Limit { get; }
	private LimitedOrderedListNode<T> Head { get; set; }
	private LimitedOrderedListNode<T> Tail { get; set; }

	public LimitedOrderedList(int limit)
	{
		Size = 0;
		Limit = limit;
		Head = null;
		Tail = null;
	}

	public T Last()
	{
		if (Tail == null)
		{
			return default;
		}

		return Tail.Item;
	}

	public T Fist()
	{
		if (Head == null)
		{
			return default;
		}

		return Head.Item;
	}

	public void Iterate(Action<int, T> iterator)
	{
		var index = 0;
		var actual = Head;

		while (actual != null)
		{
			iterator(index, actual.Item);
			actual = actual.Next;
			index++;
		}
	}

	public void Add(T item)
	{
		var listItem = new LimitedOrderedListNode<T>(item);

		if (Size == 0)
		{
			Head = listItem;
			Tail = listItem;
			Size++;

			return;
		}

		Determinate(listItem, item);
	}

	private void Displace(LimitedOrderedListNode<T> node, LimitedOrderedListNode<T> by)
	{
		if (node == Head)
		{
			Head = by;
			by.Next = node;
			node.Previous = by;
		}

		if (node.Previous != null)
		{
			node.Previous.Next = by;
			by.Previous = node.Previous;
		}

		by.Next = node;
		node.Previous = by;

		Size++;
	}

	private void RemoveNode(LimitedOrderedListNode<T> node)
	{
		if (node == Head)
		{
			Head = node.Next;
		}

		if (node == Tail)
		{
			Tail = node.Previous;
		}

		if (node.Previous != null)
		{
			node.Previous.Next = node.Next;
		}

		if (node.Next != null)
		{
			node.Next.Previous = node.Previous;
		}

		Size--;
	}

	private void Determinate(LimitedOrderedListNode<T> listItem, T item)
	{
		// Tail should never be null at this point
		// So we check Tail validity to calm down the compiler
		if (Tail == null)
		{
			throw new Exception("Interanal fatal error: illegal state");
		}

		LimitedOrderedListNode<T> found = FindFirst(i =>
		{
			return i.Compare(item) < 0 || i.Compare(item) == 0;
		});

		if (found == null)
		{
			if (Size < Limit)
			{
				Tail.Next = listItem;
				listItem.Previous = Tail;

				Tail = listItem;
				Size++;
			}

			return;
		}

		Displace(found, listItem);

		if (Size > Limit)
		{
			RemoveNode(Tail);
		}
	}

	private LimitedOrderedListNode<T> FindFirst(Func<T, bool> finder)
	{
		LimitedOrderedListNode<T> actual = Head;

		while (actual != null)
		{
			if (finder(actual.Item))
			{
				break;
			}

			actual = actual.Next;
		}

		return actual;
	}
}
