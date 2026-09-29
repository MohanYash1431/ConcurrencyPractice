using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer;

public class BoundedBuffer<T>
{
    private readonly Queue<T> _queue = new Queue<T>();
    private readonly int _capacity;
    private readonly object _lock = new object();

    public BoundedBuffer(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
        }

        _capacity = capacity;
    }

    public void Add(T item)
    {
        lock (_lock)
        {
            while (_queue.Count >= _capacity)
            {
                Console.WriteLine("Buffer is full, waiting...");
                Monitor.Wait(_lock);
            }
            _queue.Enqueue(item);
            Console.WriteLine("Item added to buffer.");
            Monitor.PulseAll(_lock);
        }
    }

    public T Remove()
    {
        lock (_lock)
        {
            while (_queue.Count == 0)
            {   
                Console.WriteLine("Buffer is empty, waiting...");
                Monitor.Wait(_lock);
            }
            T item = _queue.Dequeue();
            Console.WriteLine("Item removed from buffer.");     
            Monitor.PulseAll(_lock);
            return item;
        }
    }
}