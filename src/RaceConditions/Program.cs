using System;
using System.Threading;


int counter = 0; //Shared by both workers
//object lockObj = new object();
const int iterations = 1_000_000;

Thread worker1 = new Thread(IncrementCounter);
Thread worker2 = new Thread(IncrementCounter);


worker1.Start();
worker2.Start();

worker1.Join();
worker2.Join();

// Print results only after both workers finish
Console.WriteLine($"Expected: {iterations * 2:N0}");
Console.WriteLine($"Actual:   {counter:N0}");
Console.WriteLine($"Lost updates: {iterations * 2 - counter:N0}");


void IncrementCounter()
{
    for (int i = 0; i < iterations; i++)
    {
        //lock (lockObj)
        //{
            // counter++;
        //}

        Interlocked.Increment(ref counter);
    }
}


Console.WriteLine($"Final counter value: {counter}");