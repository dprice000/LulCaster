using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LulCaster.Utility.Common.Aggregation
{
  public class CollapsingFeedAggregate
  {
    private IList<FeedLine> _previousState = new List<FeedLine>();

    public IEnumerable<FeedLine> Analyze(IList<FeedLine> feedLines)
    {
      _previousState = feedLines;
      return feedLines.Except(_previousState);
    }
  }
}
