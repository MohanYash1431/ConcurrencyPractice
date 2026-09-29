using ProducerConsumer;

var buffer = new BoundedBuffer<int>(3);
const int itemCount = 10;

Thread producer = new Thread(() =>
{
    for (int i = 1; i <= itemCount; i++)
    {
        buffer.Add(i);
        Thread.Sleep(500); // Fast producer
    }
});

Thread consumer = new Thread(() =>
{
    for (int i = 1; i <= itemCount; i++)
    {
        int item = buffer.Remove();
        Thread.Sleep(100); // Slow consumer
    }
});

Console.WriteLine("Starting producer and consumer...");

producer.Start();
consumer.Start();

producer.Join();
consumer.Join();

Console.WriteLine("All 10 items produced and consumed.");