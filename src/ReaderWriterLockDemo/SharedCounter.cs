using System;
using System.Threading; 

namespace ReaderWriterLockDemo;

public class SharedCounter : IDisposable
{
    private int _value = 0;
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public int Read()
    {
        _lock.EnterReadLock();
       
       try
       {
           Console.WriteLine($"{DateTime.Now:HH:mm:ss:fff} [{Thread.CurrentThread.ManagedThreadId}] Read value: {_value}"); 
           
           Thread.Sleep(1000); // Simulate some delay for reading

           Console.WriteLine($"{DateTime.Now:HH:mm:ss:fff} [{Thread.CurrentThread.ManagedThreadId}] Finished reading value: {_value}"); 
          return _value;   
       }
       finally
       {
           _lock.ExitReadLock();
       }
    }
    
    public void Write(int newValue)
   {
    Console.WriteLine(
        $"{DateTime.Now:HH:mm:ss.fff} [{Thread.CurrentThread.ManagedThreadId}] WRITE REQUEST");

    _lock.EnterWriteLock();

    try
    {
        Console.WriteLine(
            $"{DateTime.Now:HH:mm:ss.fff} [{Thread.CurrentThread.ManagedThreadId}] WRITE START");

        Thread.Sleep(1000); // Simulate work while holding the write lock
        _value = newValue;

        Console.WriteLine(
            $"{DateTime.Now:HH:mm:ss.fff} [{Thread.CurrentThread.ManagedThreadId}] WRITE END: {_value}");
    }
    finally
    {
        _lock.ExitWriteLock();
    }
    }

    public void Dispose()
    {
        _lock.Dispose();
    }
}