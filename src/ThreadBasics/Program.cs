using System;
using System.Threading;

Console.WriteLine($"Main Started | Thread {Environment.CurrentManagedThreadId}");   

Thread worker1 = new Thread(() => DoWork("Worker A"));
Thread worker2 = new Thread(() => DoWork("Worker B"));

worker1.Start();
worker1.Join();

worker2.Start();
worker2.Join(); 

Console.WriteLine($"both workers finished and Main Thread Continues | Thread {Environment.CurrentManagedThreadId}");

void DoWork(string workerName)
{
    for(int i = 1 ; i < 5; i++)
    {
        Console.WriteLine($"{workerName} Working {i} | Thread {Environment.CurrentManagedThreadId}");
        Thread.Sleep(500); // Simulate work
    }
}



Console.WriteLine($"Main Finished | Thread {Environment.CurrentManagedThreadId}");