using ProducerConsumer;

// Create a bounded buffer with a capacity of 3
var buffer = new BoundedBuffer<int>(3);

const int itemCount = 10;

// Add items to the buffer
// buffer.Add(10);
// buffer.Add(20);
// buffer.Add(30);  

// Remove items from the buffer
// var removedItem1 = buffer.Remove();
// var removedItem2 = buffer.Remove();
// var removedItem3 = buffer.Remove();

// Console.WriteLine("Bounded buffer created with capacity 3.");
// Console.WriteLine($"Removed items: {removedItem1}, {removedItem2}, {removedItem3}");

Thread producer = new Thread(() =>
{
    for (int i = 1; i <= itemCount; i++)
    {
        buffer.Add(i);
        Thread.Sleep(100); // Simulate some delay between producing items
    }
});

Thread consumer = new Thread(() =>
{
    for (int i = 1; i <= itemCount; i++)
    {
        buffer.Remove();
        Thread.Sleep(500); // Simulate some delay between consuming items
    }
});

producer.Start();
consumer.Start();

producer.Join();
consumer.Join();

Console.WriteLine("All items have been produced and consumed.");
