using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer;

public class BoundedBuffer<T>
{
    //Queue stores items in FIFO (First In, First Out) order
    private readonly Queue<T> _queue = new();

    //Maximum Number of items allowed in the Queue  
    private readonly int _capacity;

    // Shared object used to Co-oridinate Threads accessing the buffer
    private readonly object _lock = new();


    // Constructor to initialize the bounded buffer with a specified capacity
    public BoundedBuffer(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
        }

        _capacity = capacity;
    }

    // Add a Method to add an item to the buffer which called by the producer thread
    public void Add(T item)
    {
        lock (_lock)
        {
            while (_queue.Count >= _capacity)
            {
                Console.WriteLine("Buffer full. Producer waiting...");

                Monitor.Wait(_lock);
            }

            _queue.Enqueue(item);

            Console.WriteLine($"Added: {item} | Size: {_queue.Count}/{_capacity}");

            Monitor.PulseAll(_lock);
        }
    }

    // Add a Method to remove an item from the buffer which called by the consumer thread
    public T Remove()
    {
        lock (_lock)
        {
            while (_queue.Count == 0)
            {
                Console.WriteLine("Buffer empty. Consumer waiting...");

                Monitor.Wait(_lock);
            }

            T item = _queue.Dequeue();

            Console.WriteLine($"Removed: {item} | Size: {_queue.Count}/{_capacity}");

            Monitor.PulseAll(_lock);

            return item;
        }
    }
}
