using System;
using System.Threading;
using CustomReadWriteLockDemo;

var counter = new SharedCounter();

Thread reader1 = new Thread(() => counter.Read())
{
    Name = "Reader-1"
};

Thread reader2 = new Thread(() => counter.Read())
{
    Name = "Reader-2"
};

Thread writer = new Thread(() => counter.Write(99))
{
    Name = "Writer"
};

Thread lateReader = new Thread(() => counter.Read())
{
    Name = "Late-Reader"
};

reader1.Start();
reader2.Start();

Thread.Sleep(200);
writer.Start();

Thread.Sleep(200);
lateReader.Start();

reader1.Join();
reader2.Join();
writer.Join();
lateReader.Join();

Console.WriteLine($"Final value: {counter.Read()}");