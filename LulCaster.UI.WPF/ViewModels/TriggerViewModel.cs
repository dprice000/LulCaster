using LulCaster.Utility.Common.Logic;
using System.Media;

namespace LulCaster.UI.WPF.ViewModels
{
  public class TriggerViewModel : ViewModelBase
  {
    private string _name;
    private TriggerTypes _triggerType;
    private string _thresholdValue;
    private string _soundFilePath;


    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        _name = value;
        OnPropertyChanged(nameof(Name));
      }
    }

    public TriggerTypes TriggerType
    {
      get
      {
        return _triggerType;
      }
      set
      {
        _triggerType = value;
        OnPropertyChanged(nameof(TriggerType));
      }
    }

    public string ThresholdValue
    {
      get
      {
        return _thresholdValue;
      }
      set
      {
        _thresholdValue = value;
        OnPropertyChanged(nameof(ThresholdValue));
      }
    }

    public string SoundFilePath
    {
      get
      {
        return _soundFilePath;
      }
      set
      {
        _soundFilePath = value;
        SoundFile = new SoundPlayer(_soundFilePath);
        OnPropertyChanged(nameof(SoundFilePath));
      }
    }

    public SoundPlayer SoundFile { get; private set; }
  }
}