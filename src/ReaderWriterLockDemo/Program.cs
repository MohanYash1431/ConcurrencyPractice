using System;
using System.Threading;
using ReaderWriterLockDemo;

using var counter = new SharedCounter();

Thread reader1 = new Thread(() => counter.Read());
Thread reader2 = new Thread(() => counter.Read());
Thread writer = new Thread(() => counter.Write(99));

reader1.Start();
reader2.Start();

Thread.Sleep(200); // Give readers a head start for this simulation
writer.Start();

reader1.Join();
reader2.Join();
writer.Join();

// Main reads the value after all workers finish
Console.WriteLine($"Final value: {counter.Read()}");