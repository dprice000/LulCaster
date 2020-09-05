namespace LulCaster.Utility.Common.Logic.Triggers
{
  public class TextTrigger : TriggerBase, ITrigger
  {
    private bool results; 

    public override bool Evaluate()
    {
      throw new System.NotImplementedException();
    }

    //TODO: Eventually this needs to be reworked to work with patterns to allow for text templates.
    public bool Process(string scrappedText, string expectedText)
    {
      return scrappedText == expectedText;
    }
  }
}