using System;
using System.Collections.Concurrent;
using System.Threading;

using var buffer = new BlockingCollection<int>(3);

const int itemCount = 10;

Thread producer = new Thread(() =>
{
    for (int i = 1; i <= itemCount; i++)
    {
        buffer.Add(i); // waits automatically when the buffer is full
        Console.WriteLine($"Produced item: {i}");
        Thread.Sleep(100); // Simulate some delay between producing items
    }

    buffer.CompleteAdding(); // signal that no more items will be added
    Console.WriteLine("Producer has finished producing all items.");
});

Thread consumer = new Thread(() =>
{
    foreach (var item in buffer.GetConsumingEnumerable())
    {
        Console.WriteLine($"Consumed item: {item}");
        Thread.Sleep(500); // Simulate some delay between consuming items
    }

    Console.WriteLine("Consumer has finished consuming all items.");
});

producer.Start();
consumer.Start();   

producer.Join();
consumer.Join();

Console.WriteLine("All items have been produced and consumed.");