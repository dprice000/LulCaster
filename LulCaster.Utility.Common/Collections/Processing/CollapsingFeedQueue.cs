using LulCaster.Utility.Common.Collections.Processing.Events;
using System.Collections.Generic;
using System.Linq;

namespace LulCaster.Utility.Common.Collections.Processing
{
  public class CollapsingFeedQueue
  {
    private Queue<KillEventArgs> _killQueue = new Queue<KillEventArgs>();

    public CollapsingFeedQueue()
    {
    }

    public CollapsingFeedQueue(IEnumerable<KillEventArgs> killEvents)
    {
      killEvents.ToList().ForEach(killEvent => _killQueue.Enqueue(killEvent));
    }


  }
}