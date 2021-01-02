using System.Collections.Generic;

namespace LulCaster.Utility.Common.Collections
{
  /// <summary>
  /// The .NET object "ConcurrentQueue" has known preformance issues if not used for exact case.
  /// So this class was created to provide a simpler object.
  /// </summary>
  /// <typeparam name="T">The data type of the objects saved in the queue.</typeparam>
  public class ThreadSafeQueue<T>
  {
    private object _queueLock = new object();
    private Queue<T> _queue { get; } = new Queue<T>();

    public bool IsEmpty
    {
      get
      {
        lock (_queueLock)
        {
          return _queue.Count == 0;
        }
      }
    }

    public void Enqueue(T newItem)
    {
      lock (_queueLock)
      {
        _queue.Enqueue(newItem);
      }
    }

    public T Dequeue()
    {
      lock (_queueLock)
      {
        return _queue.Dequeue();
      }
    }

  }
}