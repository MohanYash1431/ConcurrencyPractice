using System;
using System.Threading;
using CustomThreadPoolDemo;

var pool = new CustomThreadPool(3);

for (int i = 1; i <= 10; i++)
{
    int jobId = i;

    pool.Submit(() =>
    {
        Console.WriteLine(
            $"{DateTime.Now:HH:mm:ss.fff} [{Thread.CurrentThread.Name}] Job {jobId} START");

        Thread.Sleep(1000); // Simulate work

        Console.WriteLine(
            $"{DateTime.Now:HH:mm:ss.fff} [{Thread.CurrentThread.Name}] Job {jobId} END");
    });
}

// All jobs have been submitted; finish the queued work.
pool.Shutdown();
pool.AwaitTermination();

Console.WriteLine("All 10 jobs finished. Pool terminated.");