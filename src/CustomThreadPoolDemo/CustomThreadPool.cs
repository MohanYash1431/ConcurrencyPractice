using System;
using System.Collections.Generic;
using System.Threading;

namespace CustomThreadPoolDemo;

public class CustomThreadPool
{
    public CustomThreadPool(int workerCount)
{
    if (workerCount <= 0)
    {
        throw new ArgumentOutOfRangeException(nameof(workerCount));
    }

    for (int i = 0; i < workerCount; i++)
    {
        Thread worker = new Thread(WorkerLoop)
        {
            Name = $"Worker-{i + 1}",
            IsBackground = true
        };

        _workers.Add(worker);
        worker.Start();
    }
}

    private readonly Queue<Action> _taskQueue = new();
    private readonly List<Thread> _workers = new();
    private readonly object _sync = new();

    private bool _isShutdown = false;

    public void Submit(Action task)
    {
        ArgumentNullException.ThrowIfNull(task);

        lock (_sync)
        {
            if (_isShutdown)
            {
                throw new InvalidOperationException(
                    "The pool is shut down and cannot accept new jobs.");
            }

            _taskQueue.Enqueue(task);

            Monitor.Pulse(_sync);
        }
    }

    private void WorkerLoop()
{
    while (true)
    {
        Action task;

        lock (_sync)
        {
            while (_taskQueue.Count == 0 && !_isShutdown)
            {
                Monitor.Wait(_sync);
            }

            if (_taskQueue.Count == 0 && _isShutdown)
            {
                return; // No remaining jobs; this worker can finish
            }

            task = _taskQueue.Dequeue();
        }

        // Execute the job AFTER releasing the queue lock.
        try
        {
            task();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[{Thread.CurrentThread.Name}] Job failed: {ex.Message}");
        }
    }
}

public void Shutdown()
{
    lock (_sync)
    {
        _isShutdown = true;

        // Wake all idle workers so they can check the shutdown state.
        Monitor.PulseAll(_sync);
    }
}

public void AwaitTermination()
{
    foreach (Thread worker in _workers)
    {
        worker.Join();
    }
}
}