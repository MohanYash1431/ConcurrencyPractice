using System;
using System.Threading;
using System.Threading.Tasks;

Task[] tasks = new Task[10];

for (int i = 0; i < tasks.Length; i++)
{
    int jobId = i + 1; // Each job captures its own number

    tasks[i] = Task.Run(() =>
    {
        int threadId = Environment.CurrentManagedThreadId;
        bool isPoolThread = Thread.CurrentThread.IsThreadPoolThread;

        Console.WriteLine(
            $"{DateTime.Now:HH:mm:ss.fff} Job {jobId} START | Thread {threadId} | Pool: {isPoolThread}");

        Thread.Sleep(1000); // Simulate synchronous work occupying a worker

        Console.WriteLine(
            $"{DateTime.Now:HH:mm:ss.fff} Job {jobId} END   | Thread {threadId}");
    });
}

await Task.WhenAll(tasks);

Console.WriteLine("All 10 jobs finished.");