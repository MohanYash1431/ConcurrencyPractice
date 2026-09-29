using System;
using System.Threading;

namespace CustomReadWriteLockDemo;

public class SharedCounter
{
    private int _value = 0;
    private readonly CustomReadWriteLock _rwLock = new();

    public int Read()
    {
        Log("READ REQUEST");
        _rwLock.LockRead();

        try
        {
            Log($"READ START: {_value}");
            Thread.Sleep(1000);
            Log($"READ END: {_value}");

            return _value;
        }
        finally
        {
            _rwLock.UnlockRead();
        }
    }

    public void Write(int newValue)
    {
        Log("WRITE REQUEST");
        _rwLock.LockWrite();

        try
        {
            Log("WRITE START");
            Thread.Sleep(1000);

            _value = newValue;
            Log($"WRITE END: {_value}");
        }
        finally
        {
            _rwLock.UnlockWrite();
        }
    }

    private static void Log(string message)
    {
        string name = Thread.CurrentThread.Name ?? "Main";

        Console.WriteLine(
            $"{DateTime.Now:HH:mm:ss.fff} [{name}] {message}");
    }
}