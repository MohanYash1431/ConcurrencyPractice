using System;
using System.Threading;

namespace CustomReadWriteLockDemo;

public class CustomReadWriteLock
{
    private readonly object _sync = new();

    private int _activeReaders = 0;
    private bool _activeWriter = false;
    private int _waitingWriters = 0;

    public void LockRead()
    {
        lock (_sync)
        {
            while (_activeWriter || _waitingWriters > 0)
            {
                Monitor.Wait(_sync);
            }
            _activeReaders++;
        }
    }

    public void UnlockRead()
    {
        lock (_sync)
        {
            _activeReaders--;
            if (_activeReaders == 0)
            {
                Monitor.PulseAll(_sync);
            }
        }
    }

    public void LockWrite()
    {
        lock (_sync)
        {
            _waitingWriters++;
            try
            {
                while (_activeWriter || _activeReaders > 0)
                {
                    Monitor.Wait(_sync);
                }
                _activeWriter = true;
            }
            finally
            {
                  _waitingWriters--;

            // Waiting status changed; let waiting threads recheck.
            Monitor.PulseAll(_sync);
            }
        }
    }

    public void UnlockWrite()
    {
        lock (_sync)
        {
            _activeWriter = false;
            Monitor.PulseAll(_sync);
        }
    }

}